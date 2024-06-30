namespace GttTimeTracker.Models;

public class TimeTrackingEntry(string task, DateTime start, DateTime? end = null)
{
    public string Task { get; } = task;
    public DateTime Start { get; } = start;
    public DateTime? End { get; set; } = end;

    public double GetTotalMinutes()
    {
        return ((End ?? DateTime.Now) - Start).TotalMinutes;
    }
}