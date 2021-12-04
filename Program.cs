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
        private const string GitBinaryName = "git";

        public static void Main(string[] args)
        {
            try
            {
                ICommand command = (args.FirstOrDefault() ?? Help.Command) switch
                    {
                        Checkout.Command => new Checkout(SetupEntryStorage()),
                        Help.Command => new Help(),
                        TaskOverview.Command => new TaskOverview(SetupEntryStorage()),
                        Today.Command => new Today(SetupEntryStorage()),
                        _ => new ForwardToGit()
                    };

                command.HandleAsync(args.Skip(1))
                    .GetAwaiter()
                    .GetResult();

                if (command.ContinueToGit)
                {
                    Process.Start(GitBinaryName, args).WaitForExit();
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        private static IEntryStorage SetupEntryStorage()
        {
            var gitDir = GitProvider.FindGitDirectory(new DirectoryInfo(Directory.GetCurrentDirectory()));

            if (string.IsNullOrWhiteSpace(gitDir))
            {
                throw new Exception("Fatal: Not in a git repository!");
            }

            return EntryStorage.GetInstanceAsync($"{gitDir}/{GttFileName}")
                .GetAwaiter()
                .GetResult();
        }
    }
}
