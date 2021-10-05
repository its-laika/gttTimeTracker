using System.IO;
using System.Linq;

namespace GttTimeTracker.Services
{
    public static class GitProvider
    {
        private const char DirectorySeparator = '\\';
        private const string GitDirectoryName = ".git";

        public static string? FindGitDirectory()
        {
            var directories = Directory.GetCurrentDirectory().Split(DirectorySeparator).ToList();

            while (directories.Any())
            {
                var possibleGitDirectory = string.Join(DirectorySeparator, directories) + DirectorySeparator + GitDirectoryName;
                
                if (Directory.Exists(possibleGitDirectory))
                {
                    return possibleGitDirectory;
                }
                
                directories.RemoveAt(directories.Count - 1);
            }

            return null;
        }

        public static string? FindGitBinary()
        {
            return "git"; // TODO
        }
    }
}