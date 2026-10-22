using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Infrastructure.AI
{
    public static class CosineSimilarity
    {
        public static double Calculate(
            ReadOnlySpan<float> vectorA,
            ReadOnlySpan<float> vectorB)
        {
            if (vectorA.Length != vectorB.Length)
            {
                throw new ArgumentException(
                    "Vectors must have the same dimension.");
            }

            double dotProduct = 0;
            double magnitudeA = 0;
            double magnitudeB = 0;

            for (var i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];

                magnitudeA += vectorA[i] * vectorA[i];

                magnitudeB += vectorB[i] * vectorB[i];
            }

            if (magnitudeA == 0 || magnitudeB == 0)
            {
                return 0;
            }
            //              A · B
            //   cosine = ───────────────
            //            || A || × || B ||

            return dotProduct /
                   (Math.Sqrt(magnitudeA) *
                    Math.Sqrt(magnitudeB));
        }
    }
}
