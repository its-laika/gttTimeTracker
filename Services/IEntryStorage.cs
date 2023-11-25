namespace GttTimeTracker.Services;

public interface IEntryStorage
{
    IEnumerable<TimeTrackingEntry> Entries { get; }
    void Add(TimeTrackingEntry entry);
    void Remove(IEnumerable<TimeTrackingEntry> entries);
    Task StoreAsync();
}