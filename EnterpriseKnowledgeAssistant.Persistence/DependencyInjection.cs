using EnterpriseKnowledgeAssistant.Application.Abstractions.Persistence;
using EnterpriseKnowledgeAssistant.Persistence.Context;
using EnterpriseKnowledgeAssistant.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,  IConfiguration configuration)
        {
            services.AddDbContext<KnowledgeDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("KnowledgeDb")));

            services.AddScoped<IKnowledgeDocumentRepository, KnowledgeDocumentRepository>();

            services.AddScoped< IDocumentChunkRepository,DocumentChunkRepository>();

            return services;
        }
    }
}
