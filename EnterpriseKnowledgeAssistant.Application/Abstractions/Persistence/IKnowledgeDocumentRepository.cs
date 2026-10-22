using EnterpriseKnowledgeAssistant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Persistence
{
    public interface IKnowledgeDocumentRepository
    {
        Task AddAsync(
            KnowledgeDocument document,
            CancellationToken cancellationToken = default);

        Task<KnowledgeDocument?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(
    KnowledgeDocument document,
    CancellationToken cancellationToken = default);
    }
}
