using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GttTimeTracker.Models;
using GttTimeTracker.Services;

namespace GttTimeTracker.Commands
{
    public class Today : ICommand
    {
        public const string Command = "today";
        public bool ContinueToGit => false;

        private readonly IEntryStorage _entryStorage;

        public Today(IEntryStorage entryStorage)
        {
            _entryStorage = entryStorage;
        }

        public Task HandleAsync(IEnumerable<string> parameters)
        {
            var today = DateTime.Now.Date;

            var entries = _entryStorage.Entries
                .Where(e => e.Start.Date == today)
                .ToList();

            Console.WriteLine();
            Console.WriteLine("All Tasks:");
            foreach (var entry in entries)
            {
                var end = entry.End?.ToString("u") ?? "now";
                Console.WriteLine($"{entry.Task}: from {entry.Start:u} until {end}.");
            }

            Console.WriteLine();
            Console.WriteLine("Accumulation:");
            foreach (var taskEntries in entries.GroupBy(e => e.Task))
            {
                var (hours, minutes) = CalculateTotalHoursAndMinutes(taskEntries);
                Console.WriteLine($"{taskEntries.Key}: {hours} hour(s) {minutes} minute(s)");
            }

            var (totalHours, totalMinutes) = CalculateTotalHoursAndMinutes(entries);
            Console.WriteLine();
            Console.WriteLine($"Total: {totalHours} hour(s) {totalMinutes} minute(s)");

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