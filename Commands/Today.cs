namespace GttTimeTracker.Commands;

public class Today(IEntryStorage entryStorage) : ICommand
{
    public const string COMMAND = "today";
    public bool ContinueToGit => false;

    public Task HandleAsync(IReadOnlyList<string> parameters)
    {
        var today = DateTime.Now.Date;

        var entries = entryStorage.Entries
           .Where(e => e.Start.Date == today)
           .ToList();

        Console.WriteLine("\ntasks:");
        foreach (var entry in entries)
        {
            var end = entry.End?.ToString("u") ?? "now";
            Console.WriteLine($"{entry.Task} from {entry.Start:u} until {end}");
        }

        Console.WriteLine("\naccumulation:");
        foreach (var taskEntries in entries.GroupBy(e => e.Task))
        {
            var (hours, minutes) = taskEntries
               .Sum(e => e.GetTotalMinutes())
               .ToHoursAndMinutes();

            Console.WriteLine($"{taskEntries.Key}: {hours} hour(s) {minutes} minute(s)");
        }

        var (totalHours, totalMinutes) = entries
           .Sum(e => e.GetTotalMinutes())
           .ToHoursAndMinutes();

        Console.WriteLine($"\ntotal: {totalHours} hour(s) {totalMinutes} minute(s)");

        return Task.CompletedTask;
    }
}