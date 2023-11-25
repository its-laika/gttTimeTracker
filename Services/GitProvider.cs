namespace GttTimeTracker.Services;

public static class GitProvider
{
    public const string GIT_DIRECTORY_NAME = ".git";

    public static string? FindGitDirectory(DirectoryInfo directoryInfo)
    {
        foreach (var subfolderInfo in directoryInfo.GetDirectories())
        {
            if (subfolderInfo.Name == GIT_DIRECTORY_NAME)
            {
                return subfolderInfo.FullName;
            }
        }

        var parent = directoryInfo.Parent;

        return parent is not null
            ? FindGitDirectory(parent)
            : null;
    }
}