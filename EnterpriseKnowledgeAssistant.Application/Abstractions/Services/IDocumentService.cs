using EnterpriseKnowledgeAssistant.Application.Documents;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Services
{
    public interface IDocumentService
    {
        Task<DocumentUploadResponse> UploadAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            long fileSize,
            CancellationToken cancellationToken = default);
    }
}
