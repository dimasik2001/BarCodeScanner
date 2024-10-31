using BarCodeScanner.Services.Abstractions;
using Newtonsoft.Json;

namespace BarCodeScanner.Services
{
    public class FileStorageService: IStorageService
    {
        private readonly string _storagePath;

        public FileStorageService()
        {
            _storagePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }
        public async Task<Dictionary<string, T>> GetAllAsync<T>()
        {
            var data = new Dictionary<string, T>();
            var dirPath = Path.Combine(_storagePath, typeof(T).Name);
            if (!Directory.Exists(dirPath))
            {
                return data;
            }
            var files = Directory.GetFiles(dirPath);

            foreach (var file in files)
            {
                var jsonData = await File.ReadAllTextAsync(file);
                var value = JsonConvert.DeserializeObject<T>(jsonData);
                var key = Path.GetFileName(file);
                data[key] = value;
            }

            return data;
        }
        public async Task SetAsync<T>(string key, T data)
        {
            var dirPath = Path.Combine(_storagePath, typeof(T).Name);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            };
            var jsonData = JsonConvert.SerializeObject(data);
            await File.WriteAllTextAsync(Path.Combine(dirPath, key), jsonData);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var filePath = Path.Combine(_storagePath, typeof(T).Name, key);

            if (!File.Exists(filePath))
            {
                return default;
            }

            var jsonData = await File.ReadAllTextAsync(filePath);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
        public bool Remove<T>(string key)
        {
            var filePath = Path.Combine(_storagePath, typeof(T).Name, key);

            if (!File.Exists(filePath))
            {
                return false;
            }

            File.Delete(filePath);
            return true;
        }

        public Task<bool> ContainsKeyAsync<T>(string key)
        {
            var filePath = Path.Combine(_storagePath, typeof(T).Name, key);
            return Task.FromResult(File.Exists(filePath));
        }
    }
}
