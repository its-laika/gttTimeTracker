using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using GttTimeTracker.Models;

namespace GttTimeTracker.Services
{
    public class EntryStorage : IEntryStorage
    {
        public IEnumerable<TimeTrackingEntry> Entries => _entries;
        private readonly IList<TimeTrackingEntry> _entries;
        private readonly string _configPath;

        private static EntryStorage? _instance;

        private EntryStorage(string configPath, IList<TimeTrackingEntry> entries)
        {
            _configPath = configPath;
            _entries = entries;
        }

        public static async Task<EntryStorage> GetInstanceAsync(string configPath)
        {
            if (_instance is not null)
            {
                return _instance;
            }

            await using var fileStream = File.Open(configPath, FileMode.OpenOrCreate);
            using var streamReader = new StreamReader(fileStream);

            var json = await streamReader.ReadToEndAsync();

            var entries = !string.IsNullOrWhiteSpace(json)
                ? JsonSerializer.Deserialize<List<TimeTrackingEntry>>(json) ?? new List<TimeTrackingEntry>()
                : new List<TimeTrackingEntry>();

            _instance = new EntryStorage(configPath, entries);

            return _instance;
        }

        public void Add(TimeTrackingEntry entry)
        {
            _entries.Add(entry);
        }

        public void Remove(IEnumerable<TimeTrackingEntry> entries)
        {
            foreach(var entry in entries) {
                _entries.Remove(entry);
            }
        }

        public async Task StoreAsync()
        {
            var json = JsonSerializer.Serialize(Entries, new JsonSerializerOptions {WriteIndented = true});

            await using var fileStream = File.Open(_configPath, FileMode.Create);
            await using var streamWriter = new StreamWriter(fileStream);

            await streamWriter.WriteAsync(json);
            await streamWriter.FlushAsync();
        }
    }
}