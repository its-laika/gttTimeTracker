namespace GttTimeTracker.Commands;

public class Tasks(IEntryStorage entryStorage) : ICommand
{
    public const string COMMAND = "tasks";
    public bool ContinueToGit => false;

    public Task HandleAsync(IReadOnlyList<string> parameters)
    {
        var tasksTotalMapping = entryStorage.Entries
           .GroupBy(e => e.Task)
           .ToDictionary(
                g => g.Key,
                g => g.Sum(e => e.GetTotalMinutes())
            );

        Console.WriteLine();
        
        if (tasksTotalMapping.Count == 0)
        {
            Console.WriteLine("no tasks");
            return Task.CompletedTask;
        }

        Console.WriteLine("accumulation:");
        foreach (var (task, totalTime) in tasksTotalMapping)
        {
            var (hours, minutes) = totalTime.ToHoursAndMinutes();
            Console.WriteLine($"{task,-15} {hours,3} hour(s) {minutes,2} minute(s)");
        }

        return Task.CompletedTask;
    }
}