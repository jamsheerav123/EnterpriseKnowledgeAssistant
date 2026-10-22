using EnterpriseKnowledgeAssistant.Application.Abstractions.AI;
using EnterpriseKnowledgeAssistant.Application.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Application.Services
{
    public class RagService : IRagService
    {
        private readonly ISemanticSearchService _semanticSearchService;
        private readonly ILlmService _llmService;

        public RagService(
            ISemanticSearchService semanticSearchService,
            ILlmService llmService)
        {
            _semanticSearchService = semanticSearchService;
            _llmService = llmService;
        }

        public async Task<RagAnswerResponse> AskAsync(
            string question,
            int topK = 5,
            CancellationToken cancellationToken = default)
        {
            var searchResults =
                await _semanticSearchService.SearchAsync(
                    question,
                    topK,
                    cancellationToken);

            if (searchResults.Count == 0)
            {
                return new RagAnswerResponse
                {
                    Question = question,
                    Answer = "I could not find relevant information in the available documents.",
                    Sources = searchResults
                };
            }


            //augmentation happens when RagService takes the user's question + retrieved chunks
            //and builds the prompt sent to the LLM......ie here
            var prompt = BuildPrompt(question, searchResults);

            var answer =
                await _llmService.GenerateAsync(
                    prompt,
                    cancellationToken);

            return new RagAnswerResponse
            {
                Question = question,
                Answer = answer,
                Sources = searchResults
            };
        }

        private static string BuildPrompt(
            string question,
            IReadOnlyList<SemanticSearchResult> searchResults)
        {
            var contextBuilder = new StringBuilder();

            foreach (var result in searchResults)
            {
                contextBuilder.AppendLine(
                    $"[Document: {result.DocumentId}, Chunk: {result.ChunkIndex}]");

                contextBuilder.AppendLine(result.Content);
                contextBuilder.AppendLine();
            }

            return $"""
        You are an enterprise knowledge assistant.

        Answer the question using only the information explicitly
        contained in the provided context.

        Rules:
        - Do not use outside knowledge.
        - Do not guess or infer missing facts.
        - Infer relationships between adjacent chunks.
        - If the answer is explicitly present, answer directly and concisely.
        - If the answer is not supported by the context, say:
          "The information is not available in the provided documents."

        CONTEXT:
        {contextBuilder}

        QUESTION:
        {question}

        ANSWER:
        """;
        }
    }
}
