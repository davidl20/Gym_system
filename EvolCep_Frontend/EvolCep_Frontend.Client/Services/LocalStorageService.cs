using Microsoft.JSInterop;
using System.Text.Json;

namespace EvolCep_Frontend.Client.Services
{
    public class LocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetItemAsync(string key, string value)
        {
            var json = value is string s ? s : JsonSerializer.Serialize(value);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
        }

        public async Task<T?> GetItemAsync <T>(string key)
        {

            if (_jsRuntime is not IJSInProcessRuntime && _jsRuntime.GetType().Name == "UnsupportedJavaScriptRuntime")
                return default;

            try
            {
                var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
                if (string.IsNullOrEmpty(json)) return default;

                if (typeof(T) == typeof(string))
                    return (T)(object)json;

                return JsonSerializer.Deserialize<T>(json);
            }
            catch
            {
                return default;
            }
        }

        public async Task RemoveItemAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}
