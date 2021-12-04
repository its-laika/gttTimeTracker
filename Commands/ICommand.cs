using System.Collections.Generic;
using System.Threading.Tasks;

namespace GttTimeTracker.Commands
{
    public interface ICommand
    {
        bool ContinueToGit { get; }
        Task HandleAsync(IReadOnlyList<string> parameters);
    }
}