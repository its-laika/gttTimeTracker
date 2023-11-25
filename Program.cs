namespace GttTimeTracker;

internal static class Program
{
    private const string _GTT_FILE_NAME = "gtt.json";
    private const string _GIT_BINARY_NAME = "git";
    private const uint _ENTRY_COUNT_WARNING_THRESHOLD = 100;

    public static void Main(string[] args)
    {
        try
        {
            ICommand command = (args.FirstOrDefault() ?? Help.COMMAND) switch
            {
                Checkout.COMMAND => new Checkout(SetupEntryStorage()),
                Cleanup.COMMAND => new Cleanup(SetupEntryStorage()),
                Start.COMMAND => new Start(SetupEntryStorage()),
                Stop.COMMAND => new Stop(SetupEntryStorage()),
                TaskOverview.COMMAND => new TaskOverview(SetupEntryStorage()),
                Today.COMMAND => new Today(SetupEntryStorage()),
                Help.COMMAND => new Help(),
                _ => new ForwardToGit()
            };

            command.HandleAsync(args.Skip(1).ToList())
               .GetAwaiter()
               .GetResult();

            if (command.ContinueToGit)
            {
                Process.Start(_GIT_BINARY_NAME, args).WaitForExit();
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
        }
    }

    private static IEntryStorage SetupEntryStorage()
    {
        var gitDir = GitProvider.FindGitDirectory(new DirectoryInfo(Directory.GetCurrentDirectory()));

        if (string.IsNullOrWhiteSpace(gitDir))
        {
            throw new Exception(
                $"fatal: not a git repository (or any of the parent directories): {GitProvider.GIT_DIRECTORY_NAME}");
        }

        return EntryStorage.GetInstanceAsync($"{gitDir}/{_GTT_FILE_NAME}", _ENTRY_COUNT_WARNING_THRESHOLD)
           .GetAwaiter()
           .GetResult();
    }
}