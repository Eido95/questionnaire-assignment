using Microsoft.EntityFrameworkCore;
using QuestionnaireService.Models;

namespace QuestionnaireService.DbContexts;

public class QuestionnaireDbContext : DbContext
{
    public DbSet<Questionnaire> Questionnaires { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;
    
    public QuestionnaireDbContext(DbContextOptions<QuestionnaireDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Questionnaire>(builder =>
        {
            builder.HasKey(questionnaire => questionnaire.Id);
            builder.Property(questionnaire => questionnaire.Subject);
        });

        modelBuilder.Entity<Question>(builder =>
        {
            builder.HasKey(question => question.Id);
            builder
                .HasOne(question => question.Questionnaire)
                .WithMany(questionnaire => questionnaire.Questions);
            builder.Property(question => question.Text).IsRequired();
            builder.Property(question => question.Comment);
        });

        modelBuilder.Entity<Answer>(builder =>
        {
            builder.HasKey(answer => answer.Id);
            builder
                .HasOne(answer => answer.Question)
                .WithMany(question => question.Answers);
            builder.Property(answer => answer.Text);
            builder.Property(answer => answer.Score);
        });
    }
}