using EnterpriseKnowledgeAssistant.Application.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.AI
{
    public interface IRagService
    {
        Task<RagAnswerResponse> AskAsync(
            string question,
            int topK = 5,
            CancellationToken cancellationToken = default);
    }
}
