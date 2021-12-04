using System.Collections.Generic;
using System.Threading.Tasks;
using GttTimeTracker.Models;

namespace GttTimeTracker.Services
{
    public interface IEntryStorage
    {
        IEnumerable<TimeTrackingEntry> Entries { get; }
        void Add(TimeTrackingEntry entry);
        Task StoreAsync();
    }
}