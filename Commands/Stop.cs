using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GttTimeTracker.Services;

namespace GttTimeTracker.Commands
{
    public class Stop : ICommand
    {
        public const string Command = "stop";
        public bool ContinueToGit => false;

        private readonly IEntryStorage _entryStorage;

        public Stop(IEntryStorage entryStorage)
        {
            _entryStorage = entryStorage;
        }

        public async Task HandleAsync(IReadOnlyList<string> parameters)
        {
            var currentEntry = _entryStorage.Entries
                .OrderBy(t => t.Start)
                .LastOrDefault();

            if (currentEntry is null || currentEntry.End is not null) {
                await Console.Error.WriteLineAsync("fatal: There is no active task.");
                return;
            }
            
            currentEntry.End = DateTime.Now;
            await _entryStorage.StoreAsync();

            Console.WriteLine($"stopped: {currentEntry.Task} from {currentEntry.Start:u} until {currentEntry.End:u}");
        }
    }
}