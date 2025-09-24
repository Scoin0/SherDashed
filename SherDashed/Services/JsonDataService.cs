using System.Text.Json;

namespace SherDashed.Services;

public class JsonDataService
{
    private readonly JsonSerializerOptions _options;
    private readonly string _dataDirectory;

    public JsonDataService(string dataDirectory = "Data")
    {
        _dataDirectory = dataDirectory;
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        if (!Directory.Exists(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory);
        }
    }

    private string GetFilePath(string fileName)
    {
        if (!fileName.EndsWith(".json"))
        {
            fileName += ".json";
        }
        return Path.Combine(_dataDirectory, fileName);
    }

    public async Task InitializeFileAsync<T>(string fileName) where T : class, new()
    {
        var filePath = GetFilePath(fileName);
        if (!File.Exists(filePath))
        {
            var emptyData = new List<T>();
            await SaveDataAsync(fileName, emptyData);
        }
    }

    public async Task InitializeFileAsync<T>(string fileName, T defaultData) where T : class
    {
        var filePath = GetFilePath(fileName);
        if (!FileExists(filePath))
        {
            await SaveDataAsync(fileName, defaultData);
        }
    }

    public async Task<T?> LoadDataAsync<T>(string fileName) where T : class
    {
        var filePath = GetFilePath(fileName);
        if (!File.Exists(filePath))
            return null;

        try
        {
            var jsonContent = await File.ReadAllTextAsync(filePath);
            if (string.IsNullOrWhiteSpace(jsonContent))
                return null;
            return JsonSerializer.Deserialize<T>(jsonContent, _options);
        }
        catch (JsonException e)
        {
            Console.WriteLine($"Error reading json file '{fileName}' : {e.Message}");
            return null;
        }
    }

    public async Task<T> LoadDataAsync<T>(string fileName, T defaultData) where T : class
    {
        var data = await LoadDataAsync<T>(fileName);
        return data ?? defaultData;
    }

    public async Task<List<T>> LoadListAsync<T>(string fileName) where T : class
    {
        var data = await LoadDataAsync<List<T>>(fileName);
        return data ?? new List<T>();
    }

    public async Task SaveDataAsync<T>(string fileName, T data) where T : class
    {
        var filePath = GetFilePath(fileName);
        try
        {
            var jsonContent = JsonSerializer.Serialize(data, _options);
            await File.WriteAllTextAsync(filePath, jsonContent);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error when writing json file: '{fileName}' : {e.Message}");
            throw;
        }
    }

    public async Task AddToListAsync<T>(string fileName, T item) where T : class
    {
        var list = await LoadListAsync<T>(fileName);
        list.Add(item);
        await SaveDataAsync(fileName, list);
    }

    public async Task UpdateInListAsync<T>(string fileName, Func<T, bool> predicate, T updatedItem) where T : class
    {
        var list = await LoadListAsync<T>(fileName);
        var index = list.FindIndex(item => predicate(item));

        if (index >= 0)
        {
            list[index] = updatedItem;
            await SaveDataAsync(fileName, list);
        }
        else
        {
            throw new InvalidOperationException($"Item not found in '{fileName}'!");
        }
    }

    public async Task RemoveFromListAsync<T>(string fileName, Func<T, bool> predicate) where T : class
    {
        var list = await LoadListAsync<T>(fileName);
        var itemToRemove = list.FirstOrDefault(predicate);

        if (itemToRemove != null)
        {
            list.Remove(itemToRemove);
            await SaveDataAsync(fileName, list);
        }
        else
        {
            throw new InvalidOperationException($"Item not found in '{fileName}'!");
        }
    }

    public async Task<T?> FindInListAsync<T>(string fileName, Func<T, bool> predicate) where T : class
    {
        var list = await LoadListAsync<T>(fileName);
        return list.FirstOrDefault(predicate);
    } 

    public bool FileExists(string fileName)
    {
        var filePath = GetFilePath(fileName);
        return File.Exists(filePath);
    }

    public FileInfo? GetFileInfo(string fileName)
    {
        var filePath = GetFilePath(fileName);
        return File.Exists(filePath) ? new FileInfo(filePath) : null;
    }

    public string[] GetAllJsonFiles()
    {
        return Directory.GetFiles(_dataDirectory, "*.json")
            .Select(Path.GetFileNameWithoutExtension)
            .ToArray()!;
    }
}