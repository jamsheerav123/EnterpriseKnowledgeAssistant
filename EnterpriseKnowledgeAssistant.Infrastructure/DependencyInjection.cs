using EnterpriseKnowledgeAssistant.Application.Abstractions.AI;
using EnterpriseKnowledgeAssistant.Application.Abstractions.Services;
using EnterpriseKnowledgeAssistant.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using EnterpriseKnowledgeAssistant.Infrastructure.AI;

namespace EnterpriseKnowledgeAssistant.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddScoped<IPdfTextExtractor, PdfTextExtractor>();
            services.AddScoped<ITextChunker, TextChunker>();
            services.AddScoped<ISemanticSearchService,SemanticSearchService>();

            services.AddHttpClient<IEmbeddingService, OllamaEmbeddingService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:11434");
                client.Timeout = TimeSpan.FromSeconds(60);
            });

            services.AddHttpClient<ILlmService, OllamaLlmService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:11434");
                client.Timeout = TimeSpan.FromMinutes(2);
            });

            return services;
        }
    }
}