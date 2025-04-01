using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;

namespace Searching_Image.Models
{
  
    public class UnsplashService
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessKey;

        public UnsplashService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _accessKey = configuration["Unsplash:AccessKey"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", _accessKey);
        }
        public async Task<IEnumerable<UnsplashImage>> GetRandomImagesAsync()
        {
            // Replace with actual Unsplash API call for random images
            var response = await _httpClient.GetAsync("https://api.unsplash.com/photos/random?count=12&client_id=8u_c9SjZ6mYbxLdnGuvDNf8pveBNd5TZO4GChcVq2RQ");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            // Deserialize directly to an array of UnsplashImage
            var result = JsonSerializer.Deserialize<IEnumerable<UnsplashImage>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result ?? new List<UnsplashImage>();
        }

        public async Task<List<UnsplashImage>> SearchImagesAsync(string query)
        {
            var response = await _httpClient.GetAsync($"https://api.unsplash.com/search/photos?query={query}");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<UnsplashSearchResult>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result?.Results ?? new List<UnsplashImage>();
        }
        public async Task<UnsplashImage> GetImageByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"https://api.unsplash.com/photos/{id}");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UnsplashImage>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }

    public class UnsplashSearchResult
    {
        public List<UnsplashImage> Results { get; set; }
    }

}
