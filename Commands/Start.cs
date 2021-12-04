using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GttTimeTracker.Services;
using GttTimeTracker.Models;

namespace GttTimeTracker.Commands
{
    public class Start : ICommand
    {
        public const string Command = "start";
        public bool ContinueToGit => false;

        private readonly IEntryStorage _entryStorage;

        public Start(IEntryStorage entryStorage)
        {
            _entryStorage = entryStorage;
        }

        public async Task HandleAsync(IReadOnlyList<string> parameters)
        {
            var lastEntry = _entryStorage.Entries
                .OrderBy(t => t.Start)
                .LastOrDefault();

            if (lastEntry is not null && lastEntry.End is null) {
                await Console.Error.WriteLineAsync("fatal: There is an active task.");
                return;
            }

            var taskName = parameters.FirstOrDefault() ?? lastEntry?.Task;
            if (taskName is null) {
                await Console.Error.WriteLineAsync("fatal: No task specified and no task to resume.");
                await Console.Error.WriteLineAsync("usage: gtt start [<TASK>]");
                await Console.Error.WriteLineAsync("       Task can only be omitted if there's an existing task to resume.");
                return;
            }

            var entry = new TimeTrackingEntry(taskName);
            _entryStorage.Add(entry);
            await _entryStorage.StoreAsync();

            Console.WriteLine($"started: {entry.Task} at {entry.Start:u}");
        }
    }
}