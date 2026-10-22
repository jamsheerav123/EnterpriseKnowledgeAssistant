using EnterpriseKnowledgeAssistant.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Text;
using UglyToad.PdfPig;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Services
{
    public class PdfTextExtractor : IPdfTextExtractor
    {
        public Task<string> ExtractTextAsync(
            Stream pdfStream,
            CancellationToken cancellationToken = default)
        {
            using var document = PdfDocument.Open(pdfStream);

            var pages = document
                .GetPages()
                .Select(page => page.Text);

            var text = string.Join(
                Environment.NewLine,
                pages);

            return Task.FromResult(text);
        }
    }
}
