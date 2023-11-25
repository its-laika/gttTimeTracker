namespace GttTimeTracker.Commands;

public class Stop(IEntryStorage entryStorage) : ICommand
{
    public const string COMMAND = "stop";
    public bool ContinueToGit => false;

    public async Task HandleAsync(IReadOnlyList<string> parameters)
    {
        var currentEntry = entryStorage.Entries.MaxBy(t => t.Start);

        if (currentEntry is not { End: null })
        {
            await Console.Error.WriteLineAsync("fatal: There is no active task.");
            return;
        }

        currentEntry.End = DateTime.Now;
        await entryStorage.StoreAsync();

        Console.WriteLine($"stopped: {currentEntry.Task} from {currentEntry.Start:u} until {currentEntry.End:u}");
    }
}