using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Services
{
    public interface IPdfTextExtractor
    {
        Task<string> ExtractTextAsync(
            Stream pdfStream,
            CancellationToken cancellationToken = default);
    }
}
