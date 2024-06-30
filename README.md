# gtt time tracker
A simple time tracking tool based on git branches

## Structure
`gtt` is a CLI tool that intercepts git commands to track times for given tasks.
Relevant commands will be forwarded to git after being handled by `gtt`. 
All necessary information is stored under `.git/gtt.json` as a JSON file.
Determination of tasks is done by a simple regex which checks for  
_(slash)(uppercase letters)(dash)(numbers)(slash or dash)_.

## Commands
These are the available `gtt` commands:

- _checkout_  
  Does a git checkout and stores information about the checkout so that the timespan for each 
  task can be calculated. The task is being determined by the branch name. If a task cannot be 
  determined, the previous task won't be paused and gtt does nothing except for forwarding the 
  checkout to git. 
- _today_  
  Lists all tasks of today, including an accumulation of today's tasks and their total time.
- _tasks_  
  Lists all tasks, including an accumulation of their total time.
- _task \<TASK>_  
  Lists all times that the given task has been checked out, including an accumulation of all 
  task times.
- _start \[\<TASK>]_  
  Starts or resumes a task. If task is omitted, the last started or checked out task will be 
  'resumed'. Otherwise time tracking for given task will be started.
  Hint: Make sure that there's no active task when starting or resuming a new task!
- _stop_  
  Stops time tracking for current task.
  Hint: Make sure that there's an active task when stopping it.
- _cleanup \<DAYS>_
  Removes all entries that ended more than given days ago.
  Hint: Discards the time so that if you `cleanup 7` on 2023-02-22 12:34, all entries are removed
  that *ended* before 2023-02-15 00:00.
- _help_  
  Shows this help and help page of git.

Any other commands will be forwarded to git. 

## License
MIT