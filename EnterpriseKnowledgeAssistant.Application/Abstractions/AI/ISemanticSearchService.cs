using EnterpriseKnowledgeAssistant.Application.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.AI
{
    public interface ISemanticSearchService
    {
        Task<IReadOnlyList<SemanticSearchResult>> SearchAsync(
            string query,
            int topK = 5,
            CancellationToken cancellationToken = default);
    }
}
