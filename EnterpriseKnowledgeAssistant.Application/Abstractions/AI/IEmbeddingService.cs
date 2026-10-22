using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.AI
{
    public interface IEmbeddingService
    {
        Task<float[]> GenerateEmbeddingAsync(       string text,           CancellationToken cancellationToken = default);
    }
}
