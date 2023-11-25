namespace GttTimeTracker.Commands;

public partial class Checkout(IEntryStorage entryStorage) : ICommand
{
    public const string COMMAND = "checkout";
    public bool ContinueToGit => true;

    /// <summary>
    /// SYNOPSIS
    /// git checkout [-q] [-f] [-m] [<branch>]
    /// git checkout [-q] [-f] [-m] --detach [<branch>]
    /// git checkout [-q] [-f] [-m] [--detach] <commit>
    /// git checkout [-q] [-f] [-m] [[-b|-B|--orphan] <new_branch>] [<start_point>]
    /// git checkout [-f|--ours|--theirs|-m|--conflict=<style>] [<tree-ish>] [--] <pathspec>...
    /// git checkout [-f|--ours|--theirs|-m|--conflict=<style>] [<tree-ish>] --pathspec-from-file=<file> [--pathspec-file-nul]
    /// git checkout (-p|--patch) [<tree-ish>] [--] [<pathspec>...] */
    /// </summary>
    private readonly IReadOnlyList<string> _parametersToIgnore = new List<string>
    {
        "-q", "-f", "-m",
        "--detach",
        "-b", "-b", "--orphan",
        "-f", "--ours", "--theirs", "-m", "--conflict=", "--",
        "--pathspec-from-file=", "--pathspec-file-nul",
        "-p", "--patch",
    };

    /// <summary>
    /// This parameter indicates a checkout for a single file / multiple files without
    /// doing an actual checkout. Therefore, commands containing this parameter should
    /// be ignored.
    /// </summary>
    private const string _PARAMETER_FOR_SINGLE_FILE_CHECKOUT = "--";

    /// <summary>
    /// Capturing group 1 contains the task identifier, e.g.
    /// "ABC-123" of "feature/ABC-123/some-stuff" or
    /// "DEF-456" of "feature/DEF-456-some-other-stuff"
    /// </summary>
    [GeneratedRegex(@"\/([A-Z]+-\d+)[\/-]")]
    private static partial Regex TaskIdentifierRegex();

    public async Task HandleAsync(IReadOnlyList<string> parameters)
    {
        if (parameters.Contains(_PARAMETER_FOR_SINGLE_FILE_CHECKOUT))
        {
            /* Not a real checkout. */
            return;
        }

        var targetBranch = parameters.FirstOrDefault(
            param => !_parametersToIgnore.Any(param.StartsWith)
        );

        if (string.IsNullOrWhiteSpace(targetBranch))
        {
            return;
        }

        if (
            TaskIdentifierRegex().Match(targetBranch) is not
            {
                Success: true,
                Groups:
                [
                    not null,
                    { Value: var taskName }
                ]
            }
        )
        {
            await Console.Error.WriteLineAsync($"fatal: Could not determine task from '{targetBranch}'.");
            return;
        }

        var currentEntry = entryStorage.Entries.MaxBy(e => e.Start);

        if (currentEntry is { End: null })
        {
            if (currentEntry.Task == taskName)
            {
                /* Current task = future task and still active. Nothing to do. */
                return;
            }

            /* Current task != future task but still active. Stop current task and create new one later. */
            currentEntry.End = DateTime.Now;
        }

        var newEntry = new TimeTrackingEntry(taskName, DateTime.Now);

        entryStorage.Add(newEntry);
        await entryStorage.StoreAsync();

        if (currentEntry is not null)
        {
            Console.WriteLine($"stopped: {currentEntry.Task} from {currentEntry.Start:u} at {currentEntry.End:u}");
        }

        Console.WriteLine($"started: {newEntry.Task} at {newEntry.Start:u}");
    }
}