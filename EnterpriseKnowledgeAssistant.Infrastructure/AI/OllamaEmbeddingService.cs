using EnterpriseKnowledgeAssistant.Application.Abstractions.AI;
using System.Net.Http.Json;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI
{
    public class OllamaEmbeddingService : IEmbeddingService
    {
        private readonly HttpClient _httpClient;

        public OllamaEmbeddingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<float[]> GenerateEmbeddingAsync(
            string text,
            CancellationToken cancellationToken = default)
        {
            var request = new
            {
                model = "nomic-embed-text",
                input = text
            };

            using var response = await _httpClient.PostAsJsonAsync(
                "/api/embed",
                request,
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<OllamaEmbeddingResponse>(
                    cancellationToken: cancellationToken);

            if (result?.Embeddings is null ||
                result.Embeddings.Length == 0)
            {
                throw new InvalidOperationException(
                    "Ollama did not return an embedding.");
            }

            return result.Embeddings[0];
        }

        private sealed class OllamaEmbeddingResponse
        {
            public float[][] Embeddings { get; set; } = [];
        }
    }
}