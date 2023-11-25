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

        Console.WriteLine();
        Console.WriteLine("tasks:");
        foreach (var entry in entries)
        {
            var end = entry.End?.ToString("u") ?? "now";
            Console.WriteLine($"{entry.Task} from {entry.Start:u} until {end}");
        }

        Console.WriteLine();
        Console.WriteLine("accumulation:");
        foreach (var taskEntries in entries.GroupBy(e => e.Task))
        {
            var (hours, minutes) = CalculateTotalHoursAndMinutes(taskEntries);
            Console.WriteLine($"{taskEntries.Key}: {hours} hour(s) {minutes} minute(s)");
        }

        var (totalHours, totalMinutes) = CalculateTotalHoursAndMinutes(entries);
        Console.WriteLine();
        Console.WriteLine($"total: {totalHours} hour(s) {totalMinutes} minute(s)");

        return Task.CompletedTask;
    }

    private static (int, int) CalculateTotalHoursAndMinutes(IEnumerable<TimeTrackingEntry> entries)
    {
        var totalTaskMinutes = (int)entries
           .Select(t => (t.End ?? DateTime.Now) - t.Start)
           .Select(timespan => timespan.TotalMinutes)
           .Sum();

        return (totalTaskMinutes / 60, totalTaskMinutes % 60);
    }
}