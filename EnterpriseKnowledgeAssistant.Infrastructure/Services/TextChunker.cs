using EnterpriseKnowledgeAssistant.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EnterpriseKnowledgeAssistant.Infrastructure.Services
{
    public class TextChunker : ITextChunker
    {
        private const int TargetChunkSize = 1500;
        private const int OverlapSize = 200;

        public IReadOnlyList<string> Chunk(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return [];

            var sentences = SplitIntoSentences(text);

            var chunks = new List<string>();
            var currentSentences = new List<string>();
            var currentLength = 0;

            foreach (var sentence in sentences)
            {
                if (currentLength + sentence.Length > TargetChunkSize &&
                    currentSentences.Count > 0)
                {
                    var chunk = string.Join(" ", currentSentences).Trim();

                    if (!string.IsNullOrWhiteSpace(chunk))
                        chunks.Add(chunk);

                    currentSentences =
                        GetOverlapSentences(currentSentences);

                    currentLength =
                        currentSentences.Sum(x => x.Length + 1);
                }

                currentSentences.Add(sentence);
                currentLength += sentence.Length + 1;
            }

            if (currentSentences.Count > 0)
            {
                var finalChunk =
                    string.Join(" ", currentSentences).Trim();

                if (!string.IsNullOrWhiteSpace(finalChunk))
                    chunks.Add(finalChunk);
            }

            return chunks;
        }

        private static List<string> SplitIntoSentences(string text)
        {
            var normalizedText =
                Regex.Replace(text, @"\s+", " ").Trim();

            return Regex
                .Split(normalizedText, @"(?<=[.!?])\s+")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }

        private static List<string> GetOverlapSentences(
            List<string> sentences)
        {
            var overlap = new List<string>();
            var totalLength = 0;

            for (var i = sentences.Count - 1; i >= 0; i--)
            {
                var sentence = sentences[i];

                if (totalLength + sentence.Length > OverlapSize &&
                    overlap.Count > 0)
                {
                    break;
                }

                overlap.Insert(0, sentence);
                totalLength += sentence.Length + 1;
            }

            return overlap;
        }
    }
}
