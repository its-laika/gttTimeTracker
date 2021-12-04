using System.Collections.Generic;
using System.Threading.Tasks;

namespace GttTimeTracker.Commands
{
    public class ForwardToGit : ICommand
    {
        public bool ContinueToGit => true;

        public Task HandleAsync(IEnumerable<string> parameters)
        {
            return Task.CompletedTask;
        }
    }
}