using EnterpriseKnowledgeAssistant.Application.Abstractions.Persistence;
using EnterpriseKnowledgeAssistant.Domain.Entities;
using EnterpriseKnowledgeAssistant.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseKnowledgeAssistant.Persistence.Repositories
{
    public class DocumentChunkRepository : IDocumentChunkRepository
    {
        private readonly KnowledgeDbContext _dbContext;

        public DocumentChunkRepository(KnowledgeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRangeAsync(
            IEnumerable<DocumentChunk> chunks,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.DocumentChunks.AddRangeAsync(
                chunks,
                cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<DocumentChunk>> GetByDocumentIdAsync(
            Guid documentId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.DocumentChunks
                .Where(x => x.DocumentId == documentId)
                .OrderBy(x => x.ChunkIndex)
                .ToListAsync(cancellationToken);
        }
        public async Task<IReadOnlyList<DocumentChunk>> GetAllAsync(
    CancellationToken cancellationToken = default)
        {
            return await _dbContext.DocumentChunks
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
