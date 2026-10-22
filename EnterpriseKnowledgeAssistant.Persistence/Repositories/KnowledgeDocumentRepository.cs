using EnterpriseKnowledgeAssistant.Application.Abstractions.Persistence;
using EnterpriseKnowledgeAssistant.Domain.Entities;
using EnterpriseKnowledgeAssistant.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Persistence.Repositories
{
    public class KnowledgeDocumentRepository     : IKnowledgeDocumentRepository
    {
        private readonly KnowledgeDbContext _dbContext;

        public KnowledgeDocumentRepository(
            KnowledgeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(
            KnowledgeDocument document,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.KnowledgeDocuments
                .AddAsync(document, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<KnowledgeDocument?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.KnowledgeDocuments
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }
        public async Task UpdateAsync(
    KnowledgeDocument document,
    CancellationToken cancellationToken = default)
        {
            _dbContext.KnowledgeDocuments.Update(document);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
