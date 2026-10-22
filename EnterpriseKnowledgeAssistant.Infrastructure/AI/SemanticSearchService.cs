using EnterpriseKnowledgeAssistant.Application.Abstractions.AI;
using EnterpriseKnowledgeAssistant.Application.Abstractions.Persistence;
using EnterpriseKnowledgeAssistant.Application.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI
{
    public class SemanticSearchService : ISemanticSearchService
    {
        private readonly IEmbeddingService _embeddingService;
        private readonly IDocumentChunkRepository _chunkRepository;

        public SemanticSearchService(
            IEmbeddingService embeddingService,
            IDocumentChunkRepository chunkRepository)
        {
            _embeddingService = embeddingService;
            _chunkRepository = chunkRepository;
        }

        public async Task<IReadOnlyList<SemanticSearchResult>> SearchAsync(
            string query,
            int topK = 5,
            CancellationToken cancellationToken = default)
        {
            var queryEmbedding =
                await _embeddingService.GenerateEmbeddingAsync(
                    query,
                    cancellationToken);

            // For our first implementation, retrieve all chunks.
            var chunks =
                await _chunkRepository.GetAllAsync(
                    cancellationToken);

            var results = chunks
                .Where(x => !string.IsNullOrWhiteSpace(x.Embedding))
                .Select(x =>
                {
                    var embedding =
                        JsonSerializer.Deserialize<float[]>(
                            x.Embedding!)!;

                    var similarity =
                        CosineSimilarity.Calculate(
                            queryEmbedding,
                            embedding);

                    return new SemanticSearchResult
                    {
                        ChunkId = x.Id,
                        DocumentId = x.DocumentId,
                        ChunkIndex = x.ChunkIndex,
                        Content = x.Content,
                        Similarity = similarity
                    };
                })
                .OrderByDescending(x => x.Similarity)
                .Take(topK)
                .ToList();

            return results;
        }
    }
}
