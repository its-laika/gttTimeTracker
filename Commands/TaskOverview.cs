namespace GttTimeTracker.Commands;

public class TaskOverview(IEntryStorage entryStorage) : ICommand
{
    public const string COMMAND = "task";
    public bool ContinueToGit => false;

    public Task HandleAsync(IReadOnlyList<string> parameters)
    {
        if (
            parameters is not [var task]
            || string.IsNullOrWhiteSpace(task)
        )
        {
            Console.Error.WriteLine(
                """
                fatal: No task given.
                usage: gtt task <TASK>
                """
            );
            return Task.CompletedTask;
        }

        var entries = entryStorage.Entries
           .Where(e => e.Task == task)
           .ToList();

        Console.WriteLine();

        if (entries.Count == 0)
        {
            Console.WriteLine("no entries");
            return Task.CompletedTask;
        }

        Console.WriteLine("entries:");
        foreach (var entry in entries)
        {
            var end = entry.End?.ToString("yyyy-MM-dd hh:mm:ss") ?? "now";
            Console.WriteLine($"{entry.Task,-15} from {entry.Start:yyyy-MM-dd hh:mm:ss} until {end}");
        }

        var (totalHours, totalMinutes) = entries
           .Sum(e => e.GetTotalMinutes())
           .ToHoursAndMinutes();

        Console.WriteLine();
        Console.WriteLine($"\ntotal: {totalHours} hour(s) {totalMinutes} minute(s)");

        return Task.CompletedTask;
    }
}