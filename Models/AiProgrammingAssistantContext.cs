using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AI_Programming_Assistant.Models;

public partial class AiProgrammingAssistantContext : DbContext
{
    public AiProgrammingAssistantContext()
    {
    }

    public AiProgrammingAssistantContext(DbContextOptions<AiProgrammingAssistantContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LearningContent> LearningContents { get; set; }

    public virtual DbSet<ProgrammingLanguage> ProgrammingLanguages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersProgrammingLanguage> UsersProgrammingLanguages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-I21SEOH7;Database=ai_programming_assistant;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LearningContent>(entity =>
        {
            entity.HasKey(e => new { e.Level, e.LanguageId }).HasName("PK__learning__E83EDB60CE696A87");

            entity.ToTable("learning_content");

            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.LanguageId).HasColumnName("language_id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.Exam)
                .HasColumnType("text")
                .HasColumnName("exam");

            entity.HasOne(d => d.Language).WithMany(p => p.LearningContents)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__learning___langu__534D60F1");
        });

        modelBuilder.Entity<ProgrammingLanguage>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("PK__programm__804CF6B33D00E53F");

            entity.ToTable("programming_language");

            entity.Property(e => e.LanguageId).HasColumnName("language_id");
            entity.Property(e => e.LanguageName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("language_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370FDB131DEC");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E6164E7843DCE").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("user_name");
        });

        modelBuilder.Entity<UsersProgrammingLanguage>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LanguageId }).HasName("PK__users_pr__91BAF8648BB89212");

            entity.ToTable("users_programming_language");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.LanguageId).HasColumnName("language_id");
            entity.Property(e => e.UserLevel).HasColumnName("user_level");

            entity.HasOne(d => d.Language).WithMany(p => p.UsersProgrammingLanguages)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users_pro__langu__5070F446");

            entity.HasOne(d => d.User).WithMany(p => p.UsersProgrammingLanguages)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users_pro__user___4F7CD00D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
