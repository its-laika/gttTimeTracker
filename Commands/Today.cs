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

        if (entries.Count == 0)
        {
            Console.WriteLine("no tasks");
            return Task.CompletedTask;
        }

        Console.WriteLine("tasks:");
        foreach (var entry in entries)
        {
            var end = entry.End?.ToString("hh:mm:ss") ?? "now";
            Console.WriteLine($"{entry.Task,-15} from {entry.Start:hh:mm:ss} until {end}");
        }

        Console.WriteLine();
        Console.WriteLine("accumulation:");
        foreach (var taskEntries in entries.GroupBy(e => e.Task))
        {
            var (hours, minutes) = taskEntries
               .Sum(e => e.GetTotalMinutes())
               .ToHoursAndMinutes();

            Console.WriteLine($"{taskEntries.Key,-15} {hours,3} hour(s) {minutes,2} minute(s)");
        }

        var (totalHours, totalMinutes) = entries
           .Sum(e => e.GetTotalMinutes())
           .ToHoursAndMinutes();

        Console.WriteLine();
        Console.WriteLine($"total: {totalHours} hour(s) {totalMinutes} minute(s)");

        return Task.CompletedTask;
    }
}