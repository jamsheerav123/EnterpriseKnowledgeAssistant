using EnterpriseKnowledgeAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseKnowledgeAssistant.Persistence.Context
{
    public class KnowledgeDbContext : DbContext
    {
        public KnowledgeDbContext(
            DbContextOptions<KnowledgeDbContext> options)
            : base(options)
        {
        }

        public DbSet<KnowledgeDocument> KnowledgeDocuments =>
            Set<KnowledgeDocument>();
        public DbSet<DocumentChunk> DocumentChunks =>
    Set<DocumentChunk>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<KnowledgeDocument>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.FileName)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(x => x.ContentType)
                    .HasMaxLength(100)
                    .IsRequired();
            });

            modelBuilder.Entity<DocumentChunk>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Content)
                    .IsRequired();

                entity.HasOne(x => x.Document)
                    .WithMany()
                    .HasForeignKey(x => x.DocumentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => new
                {
                    x.DocumentId,
                    x.ChunkIndex
                });
                entity.Property(x => x.Embedding);
            });
        }
    }
}
