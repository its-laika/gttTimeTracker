using System;
// Necessary for JSON deserialization
// ReSharper disable NotNullMemberIsNotInitialized
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace GttTimeTracker.Models
{
    public class TimeTrackingEntry
    {
        public string Task { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }

        public TimeTrackingEntry(string task, DateTime? start = null, DateTime? end = null)
        {
            Task = task;
            Start = start ?? DateTime.Now;
            End = end;
        }


#nullable disable
        public TimeTrackingEntry()
        {
        }
#nullable enable
    }
}