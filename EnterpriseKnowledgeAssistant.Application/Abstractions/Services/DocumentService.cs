using EnterpriseKnowledgeAssistant.Application.Abstractions.AI;
using EnterpriseKnowledgeAssistant.Application.Abstractions.Persistence;
using EnterpriseKnowledgeAssistant.Application.Documents;
using EnterpriseKnowledgeAssistant.Domain.Entities;
using EnterpriseKnowledgeAssistant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace EnterpriseKnowledgeAssistant.Application.Abstractions.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IKnowledgeDocumentRepository _documentRepository;
        private readonly IDocumentChunkRepository _chunkRepository;
        private readonly IPdfTextExtractor _pdfTextExtractor;
        private readonly ITextChunker _textChunker;
        private readonly IEmbeddingService _embeddingService;

        public DocumentService(
     IKnowledgeDocumentRepository documentRepository,
     IDocumentChunkRepository chunkRepository,
     IPdfTextExtractor pdfTextExtractor,
     ITextChunker textChunker, IEmbeddingService embeddingService)
        {
            _documentRepository = documentRepository;
            _chunkRepository = chunkRepository;
            _pdfTextExtractor = pdfTextExtractor;
            _textChunker = textChunker;
            _embeddingService = embeddingService;
        }

        public async Task<DocumentUploadResponse> UploadAsync(    Stream fileStream,    string fileName,    string contentType,
                                long fileSize,    CancellationToken cancellationToken = default)
        {
            var document = new KnowledgeDocument
            {
                Id = Guid.NewGuid(),
                FileName = fileName,
                ContentType = contentType,
                FileSize = fileSize,
                Status = DocumentStatus.Uploaded,
                CreatedOn = DateTime.UtcNow
            };

            await _documentRepository.AddAsync(
                document,
                cancellationToken);

            try
            {
                document.Status = DocumentStatus.Processing;

                await _documentRepository.UpdateAsync(
                    document,
                    cancellationToken);

                var text = await _pdfTextExtractor.ExtractTextAsync(
                    fileStream,
                    cancellationToken);

                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new InvalidOperationException(
                        "No text could be extracted from the PDF.");
                }

                var contents = _textChunker.Chunk(text);

                if (contents.Count == 0)
                {
                    throw new InvalidOperationException(
                        "No chunks could be generated from the document.");
                }

                var chunks = new List<DocumentChunk>();

                for (var i = 0; i < contents.Count; i++)
                {
                    var content = contents[i];

                    var embedding =
                        await _embeddingService.GenerateEmbeddingAsync(
                            content,
                            cancellationToken);
                    int len = embedding.Length;

                    var chunk = new DocumentChunk
                    {
                        Id = Guid.NewGuid(),
                        DocumentId = document.Id,
                        ChunkIndex = i,
                        Content = content,
                        CharacterCount = content.Length,

                        Embedding = JsonSerializer.Serialize(embedding),

                        CreatedOn = DateTime.UtcNow
                    };

                    chunks.Add(chunk);
                }

                await _chunkRepository.AddRangeAsync(
                    chunks,
                    cancellationToken);

                document.Status = DocumentStatus.Processed;
                document.ProcessedOn = DateTime.UtcNow;

                await _documentRepository.UpdateAsync(
                    document,
                    cancellationToken);

                return new DocumentUploadResponse
                {
                    Id = document.Id,
                    FileName = document.FileName,
                    Status = document.Status.ToString(),
                    CreatedOn = document.CreatedOn
                };
            }
            catch
            {
                document.Status = DocumentStatus.Failed;

                await _documentRepository.UpdateAsync(
                    document,
                    CancellationToken.None);
                //Why CancellationToken.None in the catch?
                //if we pass CancellationToken,  that database operation can immediately be cancelled too.
                //We may fail to record:   Status = Failed

                throw;
            }
        }
    }
}
