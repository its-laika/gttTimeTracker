# GttTimeTracker [WIP]
Just a time tracking tool based on git branches

## Status
WIP

## What?!
Here's the plan: 

### Commands
`gtt checkout <BRANCH-NAME>`: gtt tries to determine a task by given branch name. 
E.g. if the branch name is _feature/MYTASK-1337/some-thing_, the task should be _MYTASK-1337_.
An entry is created for this task and the current time. As soon as another task
is checked out, the current entry is "closed", meaning that an end-time is stored.
This command will also be forwarded to `git`, resulting in a checkout.

`gtt today`: gtt lists all entries for the current day, including the accumulated time.

`gtt task <TASK>`: gtt lists all entries for given task, including the accumulated time.

`gtt <SOMETHING ELSE>`: gtt forwards this command directly to `git`.

### Internals
gtt should store all its information in the _.git/_-folder of the current repository.
(I don't know if this will work. Otherwise it should store it's information in the home-folder of the current user.)
File format will be JSON so its human readable.

### Configuration
I'm unsure on how to configure gtt. Maybe a _~/.gttconfig_-file.
I also currently don't know how to support multiple task-identifiers. A regex would be nice but I currently have no clue
on how to store them. So it will probably be jira-only as that's what I personally use.

## License
MIT