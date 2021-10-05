using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GttTimeTracker.Models;

namespace GttTimeTracker.Services
{
    public class EntryStorage
    {
        private readonly string _configPath;

        public EntryStorage(string configPath)
        {
            _configPath = configPath;
        }
        
        public async Task Write(TimeTrackingEntry entry)
        {
            var currentEntries = await LoadAll();
            var json = JsonSerializer.Serialize(currentEntries.ToList().Append(entry));

            await using var fileStream = File.Open(_configPath, FileMode.Create);
            await using var streamWriter = new StreamWriter(fileStream);

            await streamWriter.WriteAsync(json);
            await streamWriter.FlushAsync();
        }

        public async Task<IEnumerable<TimeTrackingEntry>> LoadAll()
        {
            await using var fileStream = File.Open(_configPath, FileMode.OpenOrCreate);
            using var streamReader = new StreamReader(fileStream);

            var json = await streamReader.ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<TimeTrackingEntry>();
            }
            
            return JsonSerializer.Deserialize<IEnumerable<TimeTrackingEntry>>(json) ?? new List<TimeTrackingEntry>();
        }
    }
}