using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GttTimeTracker.Commands;
using GttTimeTracker.Services;

namespace GttTimeTracker
{
    internal static class Program
    {
        private const string GttFileName = "gtt";

        public static void Main(string[] args)
        {
            var gitDir = GitProvider.FindGitDirectory(new DirectoryInfo(Directory.GetCurrentDirectory()));
            var gitBinary = GitProvider.FindGitBinaryAsync().GetAwaiter().GetResult();

            if (string.IsNullOrWhiteSpace(gitDir))
            {
                Console.Error.WriteLine("Fatal: Not in a git repository!");
                return;
            }

            if (string.IsNullOrWhiteSpace(gitBinary))
            {
                Console.Error.WriteLine("Fatal: Cannot find git binary!");
                return;
            }

            IEntryStorage GetEntryInstance() =>
                EntryStorage.GetInstanceAsync($"{gitDir}/{GttFileName}")
                    .GetAwaiter()
                    .GetResult();

            ICommand command = (args.FirstOrDefault() ?? Help.Command) switch
            {
                Checkout.Command => new Checkout(GetEntryInstance()),
                Help.Command => new Help(),
                TaskOverview.Command => new TaskOverview(GetEntryInstance()),
                Today.Command => new Today(GetEntryInstance()),
                _ => new ForwardToGit()
            };
            var arguments = args
                .Skip(1)
                .ToList();

            command.HandleAsync(arguments)
                .GetAwaiter()
                .GetResult();

            if (command.ContinueToGit)
            {
                Process.Start(gitBinary, arguments);
            }
        }
    }
}