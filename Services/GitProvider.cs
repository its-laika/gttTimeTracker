using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace GttTimeTracker.Services
{
    public static class GitProvider
    {
        private const string GitDirectoryName = ".git";
        private const string GitBinaryName = "git";

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

        public static async Task<string?> FindGitBinaryAsync()
        {
            ProcessStartInfo processStartInfo = new(GitBinaryName)
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            var process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            await process.WaitForExitAsync();

            return process.ExitCode == 1
                ? GitBinaryName
                : null;
        }
    }
}