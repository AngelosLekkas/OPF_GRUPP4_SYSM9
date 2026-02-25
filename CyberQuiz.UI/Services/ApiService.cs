using CyberQuiz.Shared.DTOs;

namespace CyberQuiz.UI.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryDto>> GetCategories()
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryDto>>("api/categories");
        }
    }
}
