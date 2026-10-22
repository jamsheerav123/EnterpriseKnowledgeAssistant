using EnterpriseKnowledgeAssistant.Application.Abstractions.AI;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI
{
    public class OllamaLlmService : ILlmService
    {
        private readonly HttpClient _httpClient;

        public OllamaLlmService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GenerateAsync(
            string prompt,
            CancellationToken cancellationToken = default)
        {
            var request = new
            {
                model = "llama3.2:3b",
                prompt,
                stream = false
            };

            using var response =
                await _httpClient.PostAsJsonAsync(
                    "/api/generate",
                    request,
                    cancellationToken);

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content
                    .ReadFromJsonAsync<OllamaGenerateResponse>(
                        cancellationToken: cancellationToken);

            if (string.IsNullOrWhiteSpace(result?.Response))
            {
                throw new InvalidOperationException(
                    "Ollama did not return a response.");
            }

            return result.Response;
        }

        private sealed class OllamaGenerateResponse
        {
            public string Response { get; set; } = string.Empty;
        }
    }
}
