using EnterpriseKnowledgeAssistant.Application.Abstractions.AI;
using EnterpriseKnowledgeAssistant.Application.Abstractions.Services;
using EnterpriseKnowledgeAssistant.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IRagService, RagService>();

            return services;
        }
    }
}
