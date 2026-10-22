using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Services
{
    public interface ITextChunker
    {
        IReadOnlyList<string> Chunk(string text);
    }
}
