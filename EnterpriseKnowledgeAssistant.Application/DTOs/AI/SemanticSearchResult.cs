using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Application.DTOs.AI
{
    public class SemanticSearchResult
    {
        public Guid ChunkId { get; set; }

        public Guid DocumentId { get; set; }

        public int ChunkIndex { get; set; }

        public string Content { get; set; } = string.Empty;

        public double Similarity { get; set; }
    }
}
