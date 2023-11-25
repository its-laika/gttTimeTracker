namespace GttTimeTracker.Services;

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

    public static async Task<EntryStorage> GetInstanceAsync(string configPath, uint entryCountWarningThreshold)
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

        if (entries.Count > entryCountWarningThreshold)
        {
            Console.WriteLine(
                $"""
                 warning: Log currently contains {entries.Count} entries. This can slow down gtt.
                          You can use `gtt cleanup` to remove outdated entries.
                          Use `gtt help` for more information about `gtt cleanup`.
                 """
            );
        }

        _instance = new EntryStorage(configPath, entries);

        return _instance;
    }

    public void Add(TimeTrackingEntry entry)
    {
        _entries.Add(entry);
    }

    public void Remove(IEnumerable<TimeTrackingEntry> entries)
    {
        foreach (var entry in entries)
        {
            _entries.Remove(entry);
        }
    }

    public async Task StoreAsync()
    {
        var json = JsonSerializer.Serialize(Entries, new JsonSerializerOptions { WriteIndented = true });

        await using var fileStream = File.Open(_configPath, FileMode.Create);
        await using var streamWriter = new StreamWriter(fileStream);

        await streamWriter.WriteAsync(json);
        await streamWriter.FlushAsync();
    }
}