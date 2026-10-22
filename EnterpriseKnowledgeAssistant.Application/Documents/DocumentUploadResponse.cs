using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Application.Documents
{
    public class DocumentUploadResponse
    {
        public Guid Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }
    }
}
