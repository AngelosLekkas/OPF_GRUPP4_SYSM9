using System.Net.Http.Json;

namespace CyberQuiz.API.Services;

public class AiService
{
    private readonly HttpClient _httpClient;

    public AiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:11434");
    }

    private class OllamaRequest
    {
        public string model { get; set; } = "phi3";
        public string prompt { get; set; } = string.Empty;
        public bool stream { get; set; } = false;
    }

    private class OllamaResponse
    {
        public string? model { get; set; }
        public string? created_at { get; set; }
        public string? response { get; set; }
        public bool done { get; set; }
    }

    public async Task<string> AskAsync(string prompt)
    {
        var request = new OllamaRequest
        {
            prompt = prompt,
            stream = false
        };

        var response = await _httpClient.PostAsJsonAsync("/api/generate", request);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
        return result?.response ?? string.Empty;
    }
}
