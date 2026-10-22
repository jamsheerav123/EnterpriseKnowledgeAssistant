using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Domain.Entities
{
    public class DocumentChunk
    {
        public Guid Id { get; set; }

        public Guid DocumentId { get; set; }

        public int ChunkIndex { get; set; }

        public string Content { get; set; } = string.Empty;

        public int CharacterCount { get; set; }

        public string? Embedding { get; set; }

        public DateTime CreatedOn { get; set; }

        public KnowledgeDocument? Document { get; set; }
    }
}
