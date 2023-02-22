using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GttTimeTracker.Services;

namespace GttTimeTracker.Commands
{
    public class Cleanup : ICommand
    {
        public const string Command = "cleanup";
        public bool ContinueToGit => false;

        private readonly IEntryStorage _entryStorage;

        public Cleanup(IEntryStorage entryStorage)
        {
            _entryStorage = entryStorage;
        }

        public async Task HandleAsync(IReadOnlyList<string> parameters)
        {
            var remainingDaysParameter = parameters.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(remainingDaysParameter))
            {
                Console.Error.WriteLine("fatal: No number of remaining days given.");
                Console.WriteLine("usage: gtt cleanup <DAYS>");
                return;
            }

            if (!byte.TryParse(remainingDaysParameter, out var remainingDays)) {
                Console.Error.WriteLine("fatal: Given remaining days are not a positive number.");
                Console.WriteLine("usage: gtt cleanup <DAYS>");
                return;
            }

            var threshold = DateTime.Now.Date.AddDays(remainingDays * -1);
            var outdatedEntries = _entryStorage.Entries.Where(e => e.End < threshold).ToList();

            _entryStorage.Remove(outdatedEntries);

            await _entryStorage.StoreAsync();

            Console.WriteLine($"removed: All tasks older than {remainingDays} days.");
        }
    }
}