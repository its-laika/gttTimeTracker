using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GttTimeTracker.Commands
{
    public class Help : ICommand
    {
        public const string Command = "help";
        public bool ContinueToGit => true;

        public Task HandleAsync(IEnumerable<string> parameters)
        {
            if (parameters.Any())
            {
                /* User probably asks for git help. */
                return Task.CompletedTask;
            }

            const string help = @"
gtt time tracker
usage: gtt <command> [<args>]

These are the available gtt commands:

   checkout          Does a git checkout and stores information about the checkout so
                     that the timespan for each task can be calculated. The task is 
                     being determined by the branch name. See below.

   today             Lists all tasks of today, including an accumulation of today's 
                     tasks and their total time.

   task <TASK>       Lists all times that the given task has been checked out, 
                     including an accumulation of all task times.

   help              Shows this help and help page of git.

Any other commands will be forwarded to git. 

Determination of tasks is done by parsing the given branch name during checkout.
A branch should be typically build like 'branch-type/task-name/some-description',
e.g. 'feature/TASK-1337/implement-something'. gtt then takes 'TASK-1337' as the
task name. A task must be named UPPERCASE, must contain a minus (-) and a number.
It must be preceded by a slash (/) and followed by either a slash (/) or a minus
(-). If a task cannot be determined, the previous task won't be paused and gtt
does nothing except for forwarding the checkout to git. 

----
Niklas Schmidt, 2021 
Licensed under MIT
----
Showing git help:
";
            Console.WriteLine(help);

            return Task.CompletedTask;
        }
    }
}