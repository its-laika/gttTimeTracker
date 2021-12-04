using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GttTimeTracker.Models;
using GttTimeTracker.Services;

namespace GttTimeTracker.Commands
{
    public class TaskOverview : ICommand
    {
        public const string Command = "task";
        public bool ContinueToGit => false;

        private readonly IEntryStorage _entryStorage;

        public TaskOverview(IEntryStorage entryStorage)
        {
            _entryStorage = entryStorage;
        }

        public Task HandleAsync(IReadOnlyList<string> parameters)
        {
            var task = parameters.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(task))
            {
                Console.Error.WriteLine("fatal: No task given.");
                Console.WriteLine("usage: gtt task <TASK>");
                return Task.CompletedTask;
            }

            var entries = _entryStorage.Entries
                .Where(e => e.Task == task)
                .ToList();

            foreach (var entry in entries)
            {
                var end = entry.End?.ToString("u") ?? "now";
                Console.WriteLine($"{entry.Task}: from {entry.Start:u} until {end}");
            }

            var (totalHours, totalMinutes) = CalculateTotalHoursAndMinutes(entries);
            Console.WriteLine();
            Console.WriteLine($"total: {totalHours} hour(s) {totalMinutes} minute(s)");

            return Task.CompletedTask;
        }

        private static (int, int) CalculateTotalHoursAndMinutes(IEnumerable<TimeTrackingEntry> entries)
        {
            var totalTaskMinutes = (int) entries
                .Select(t => (t.End ?? DateTime.Now) - t.Start)
                .Select(timespan => timespan.TotalMinutes)
                .Sum();

            return (totalTaskMinutes / 60, totalTaskMinutes % 60);
        }
    }
}