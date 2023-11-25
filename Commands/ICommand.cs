namespace GttTimeTracker.Commands;

public interface ICommand
{
    bool ContinueToGit { get; }
    Task HandleAsync(IReadOnlyList<string> parameters);
}