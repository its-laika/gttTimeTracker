namespace GttTimeTracker.Commands;

/// <summary>
/// This is just a "dummy" command that does nothing but specifies
/// that the given command should be forwarded to git.
/// Necessary to "streamline" the program structure.
/// </summary>
public class ForwardToGit : ICommand
{
    public bool ContinueToGit => true;

    public Task HandleAsync(IReadOnlyList<string> parameters)
    {
        return Task.CompletedTask;
    }
}