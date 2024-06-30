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

        foreach (var entry in entries)
        {
            var end = entry.End?.ToString("u") ?? "now";
            Console.WriteLine($"{entry.Task}: from {entry.Start:u} until {end}");
        }

        var (totalHours, totalMinutes) = entries
           .Sum(e => e.GetTotalMinutes())
           .ToHoursAndMinutes();

        Console.WriteLine($"\ntotal: {totalHours} hour(s) {totalMinutes} minute(s)");

        return Task.CompletedTask;
    }
}