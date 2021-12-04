using System.IO;

namespace GttTimeTracker.Services
{
    public static class GitProvider
    {
        private const string GitDirectoryName = ".git";

        public static string? FindGitDirectory(DirectoryInfo directoryInfo)
        {
            foreach (var subfolderInfo in directoryInfo.GetDirectories())
            {
                if (subfolderInfo.Name == GitDirectoryName)
                {
                    return subfolderInfo.FullName;
                }
            }

            var parent = directoryInfo.Parent;

            return parent is not null
                // ReSharper disable once TailRecursiveCall
                ? FindGitDirectory(parent)
                : null;
        }
    }
}
