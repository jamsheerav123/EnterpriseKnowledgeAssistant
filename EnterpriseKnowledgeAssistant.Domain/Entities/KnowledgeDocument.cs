using System;
using System.Collections.Generic;
using System.Text;
using EnterpriseKnowledgeAssistant.Domain.Enums;

namespace EnterpriseKnowledgeAssistant.Domain.Entities
{
    public class KnowledgeDocument
    {
        public Guid Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public DocumentStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ProcessedOn { get; set; }
    }
}
