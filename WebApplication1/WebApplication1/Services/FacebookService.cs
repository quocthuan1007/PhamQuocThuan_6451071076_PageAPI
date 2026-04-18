using System.Text.Json;

namespace WebApplication1.Services
{
    public class FacebookService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _accessToken;

        public FacebookService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://graph.facebook.com/v20.0/");
            _configuration = configuration;
            _accessToken = _configuration["FacebookSettings:AccessToken"] ?? string.Empty;
        }

        public async Task<string> GetPageAsync(string pageId)
        {
            var response = await _httpClient.GetAsync($"{pageId}?fields=id,name,about,category,followers_count&access_token={_accessToken}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPagePostsAsync(string pageId)
        {
            var response = await _httpClient.GetAsync($"{pageId}/posts?access_token={_accessToken}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CreatePostAsync(string pageId, string message)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("message", message),
                new KeyValuePair<string, string>("access_token", _accessToken)
            });

            var response = await _httpClient.PostAsync($"{pageId}/feed", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> DeletePostAsync(string postId)
        {
            var response = await _httpClient.DeleteAsync($"{postId}?access_token={_accessToken}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPostCommentsAsync(string postId)
        {
            var response = await _httpClient.GetAsync($"{postId}/comments?access_token={_accessToken}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPostLikesAsync(string postId)
        {
            var response = await _httpClient.GetAsync($"{postId}/likes?access_token={_accessToken}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPageInsightsAsync(string pageId)
        {
            // Mặc định lấy insights impressions và engaged_users
            var response = await _httpClient.GetAsync($"{pageId}/insights?metric=page_impressions,page_engaged_users&access_token={_accessToken}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
