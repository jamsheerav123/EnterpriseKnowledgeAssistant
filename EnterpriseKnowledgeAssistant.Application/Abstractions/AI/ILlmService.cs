using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.AI
{
    public interface ILlmService
    {
        Task<string> GenerateAsync(string prompt,     CancellationToken cancellationToken = default);
    }
}
