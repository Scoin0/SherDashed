using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SherDashed.Services;

public class JsonDataService
{
    private readonly JsonSerializerSettings _options;
    private readonly string _dataDirectory;

    public JsonDataService(string dataDirectory = "Data")
    {
        _dataDirectory = dataDirectory;
        _options = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        if (!Directory.Exists(dataDirectory))
            Directory.CreateDirectory(dataDirectory);
    }

    private string GetFilePath(string fileName) =>
        Path.Combine(_dataDirectory, fileName.EndsWith(".json") ? fileName : $"{fileName}.json");

    public async Task<T> LoadData<T>(string fileName) where T : class, new()
    {
        var filePath = GetFilePath(fileName);
        
        if (!File.Exists(filePath))
        {
            var empty = new T();
            await SaveData(fileName, empty);
            return empty;
        }

        var json = await File.ReadAllTextAsync(filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            var empty = new T();
            await SaveData(fileName, empty);
            return empty;
        }

        return JsonConvert.DeserializeObject<T>(json, _options) ?? new T();
    }
    
    public async Task SaveData<T>(string fileName, T data) where T : class
    {
        var filePath = GetFilePath(fileName);
        var json = JsonConvert.SerializeObject(data, _options);
        await File.WriteAllTextAsync(filePath, json);
    }
}