using EnterpriseKnowledgeAssistant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Persistence
{
    public interface IDocumentChunkRepository
    {
        Task AddRangeAsync(
            IEnumerable<DocumentChunk> chunks,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<DocumentChunk>> GetByDocumentIdAsync(
            Guid documentId,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<DocumentChunk>> GetAllAsync(
    CancellationToken cancellationToken = default);
    }
}
