using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Application.DTOs.AI
{
    public class RagAnswerResponse
    {
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public IReadOnlyList<SemanticSearchResult> Sources { get; set; }  = [];
    }
}
