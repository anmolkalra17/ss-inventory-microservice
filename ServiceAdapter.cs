using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ss_inventory_microservice;

public class ServiceAdapter
{
private readonly HttpClient _httpClient;

    public ServiceAdapter(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<T> GetAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<T>();
        return content ?? default!;
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, data);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<TResponse>();
        return content ?? default!;
    }

    public async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        var response = await _httpClient.PutAsJsonAsync(endpoint, data);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<TResponse>();
        return content ?? default!;
    }

    public async Task DeleteAsync(string endpoint)
    {
        var response = await _httpClient.DeleteAsync(endpoint);
        response.EnsureSuccessStatusCode();
    }
}