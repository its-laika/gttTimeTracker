using System;

namespace GttTimeTracker.Models
{
    public record TimeTrackingEntry(
        string Project,
        DateTime Start,
        DateTime? End
    );
}