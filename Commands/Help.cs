namespace GttTimeTracker.Commands;

public class Help : ICommand
{
    public const string COMMAND = "help";
    public bool ContinueToGit => true;

    public Task HandleAsync(IReadOnlyList<string> parameters)
    {
        if (parameters.Any())
        {
            /* User probably asks for git help. */
            return Task.CompletedTask;
        }

        Console.WriteLine(
            """
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
            
               start [<TASK>]    Starts or resumes a task. If task is omitted, the last started or
                                 checked out task will be 'resumed'. Otherwise time tracking for
                                 given task will be started.
                                 Hint: Make sure that there's no active task when starting or
                                 resuming a new task!
            
               stop              Stops time tracking for current task.
                                 Hint: Make sure that there's an active task when stopping it.
            
               cleanup <DAYS>    Removes all entries that ended more than given days ago.
                                 Hint: Discards the time so that if you `cleanup 7` on 2023-02-22
                                 12:34, all entries are removed that ended before 2023-02-15 00:00.
            
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
            Curly, 2021 - 2023
            Licensed under MIT
            ----

            """
        );

        return Task.CompletedTask;
    }
}