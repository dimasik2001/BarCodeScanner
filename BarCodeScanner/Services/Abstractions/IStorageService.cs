using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCodeScanner.Services.Abstractions
{
    public interface IStorageService
    {
        Task<bool> ContainsKeyAsync<T>(string key);
        Task<Dictionary<string, T>> GetAllAsync<T>();
        Task<T> GetAsync<T>(string key);
        bool Remove<T>(string key);
        Task SetAsync<T>(string key, T data);
    }
}
