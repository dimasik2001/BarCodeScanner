using BarCodeScanner.Services.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCodeScanner.Services
{
    public class SecureStorageService : IStorageService
    {
        public async Task<bool> ContainsKeyAsync<T>(string key)
        {
            return await SecureStorage.Default.GetAsync(key) != null;
        }

        public Task<Dictionary<string, T>> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var data = await SecureStorage.Default.GetAsync(key);
            if (data != null)
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            return default;
        }

        public bool Remove<T>(string key)
        {
           return SecureStorage.Default.Remove(key);
        }

        public async Task SetAsync<T>(string key, T value)
        {
            var json = JsonConvert.SerializeObject(value);
            await SecureStorage.SetAsync(key, json);
        }
    }
}
