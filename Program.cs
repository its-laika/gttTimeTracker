using System;
using System.IO;
using GttTimeTracker.Models;
using GttTimeTracker.Services;

namespace GttTimeTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" - gtt time tracking tool - ");
            Console.WriteLine("args: [" + string.Join(',', args) + "]");
            Console.WriteLine("git dir:" + GitProvider.FindGitDirectory());
            Console.WriteLine("cwd: " + Directory.GetCurrentDirectory());

            var gttFile = GitProvider.FindGitDirectory() + "/gtt";
            var entryStorage = new EntryStorage(gttFile);

            var entry = new TimeTrackingEntry("TEST-1234", DateTime.Now, null);
            entryStorage.Write(entry).GetAwaiter().GetResult();
            var storedEntries = entryStorage.LoadAll().GetAwaiter().GetResult();

            Console.WriteLine(storedEntries);
        }
    }
}