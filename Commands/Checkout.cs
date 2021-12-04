using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GttTimeTracker.Models;
using GttTimeTracker.Services;

// ReSharper disable InvalidXmlDocComment

namespace GttTimeTracker.Commands
{
    public class Checkout : ICommand
    {
        public const string Command = "checkout";
        public bool ContinueToGit => true;

        /// <summary>
        /// SYNOPSIS
        /// git checkout [-q] [-f] [-m] [<branch>]
        /// git checkout [-q] [-f] [-m] --detach [<branch>]
        /// git checkout [-q] [-f] [-m] [--detach] <commit>
        /// git checkout [-q] [-f] [-m] [[-b|-B|--orphan] <new_branch>] [<start_point>]
        /// git checkout [-f|--ours|--theirs|-m|--conflict=<style>] [<tree-ish>] [--] <pathspec>...
        /// git checkout [-f|--ours|--theirs|-m|--conflict=<style>] [<tree-ish>] --pathspec-from-file=<file> [--pathspec-file-nul]
        /// git checkout (-p|--patch) [<tree-ish>] [--] [<pathspec>...] */
        /// </summary>
        private readonly IReadOnlyList<string> _parametersToIgnore = new List<string>
        {
            "-q", "-f", "-m",
            "--detach",
            "-b", "-b", "--orphan",
            "-f", "--ours", "--theirs", "-m", "--conflict=", "--",
            "--pathspec-from-file=", "--pathspec-file-nul",
            "-p", "--patch",
        };

        /// <summary>
        /// Capturing group 1 contains the task identifier, e.g.
        /// "ABC-123" of "feature/ABC-123/some-stuff" or
        /// "DEF-456" of "feature/DEF-456-some-other-stuff"
        /// </summary>
        private readonly Regex _taskIdentifierRegex = new(@"\/([A-Z]+-\d+)[\/-]");

        private readonly IEntryStorage _entryStorage;

        public Checkout(IEntryStorage entryStorage)
        {
            _entryStorage = entryStorage;
        }

        public async Task HandleAsync(IEnumerable<string> parameters)
        {
            var targetBranch = parameters.FirstOrDefault(param => !_parametersToIgnore.Any(param.StartsWith));

            if (string.IsNullOrWhiteSpace(targetBranch))
            {
                return;
            }

            var taskIdentifierMatch = _taskIdentifierRegex.Match(targetBranch);
            if (!taskIdentifierMatch.Success)
            {
                await Console.Error.WriteLineAsync(
                    $"Fatal: Could not determine project from target '{targetBranch}'. Continuing to git.");
                return;
            }

            var taskName = taskIdentifierMatch.Groups[1].Value;

            var currentEntry = _entryStorage.Entries
                .OrderBy(e => e.Start)
                .LastOrDefault();

            if (currentEntry is not null)
            {
                if (currentEntry.Task == taskName)
                {
                    return;
                }

                currentEntry.End = DateTime.Now;
                Console.WriteLine(
                    $"Ending task {currentEntry.Task} from {currentEntry.Start:u} at {currentEntry.End:u}.");
            }

            var newEntry = new TimeTrackingEntry(taskName);
            _entryStorage.Add(newEntry);

            Console.WriteLine($"Starting task {newEntry.Task} at {newEntry.Start:u}.");

            await _entryStorage.StoreAsync();
        }
    }
}