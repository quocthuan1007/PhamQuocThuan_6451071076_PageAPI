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
            // Tạm thời test 1 chỉ số cơ bản và an toàn nhất của Fanpage mới
            var metrics = "page_post_engagements"; 
            var period = "day"; 
            
            var url = $"{pageId}/insights?metric={metrics}&period={period}&access_token={_accessToken}";
            
            var response = await _httpClient.GetAsync(url);
            
            // Đọc chi tiết lỗi từ Facebook trả về thay vì dùng EnsureSuccessStatusCode
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Facebook API Error: {errorContent}");
            }
            
            return await response.Content.ReadAsStringAsync();
        }
    }
}
