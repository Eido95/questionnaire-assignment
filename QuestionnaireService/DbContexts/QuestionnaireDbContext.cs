using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestionnaireService.Dtos;
using QuestionnaireService.Models;

namespace QuestionnaireService.DbContexts;

public class QuestionnaireDbContext : DbContext
{
    public DbSet<Questionnaire> Questionnaires { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;
    public DbSet<Respondent> Respondents { get; set; }
    public DbSet<RespondentAnswer> RespondentsAnswers { get; set; }

    public QuestionnaireDbContext(DbContextOptions<QuestionnaireDbContext> options) : base(options) { }
    
    public ActionResult<IEnumerable<RespondentDto>> GetRespondentDtos()
    {
        return Respondents
            .Select(respondent => new RespondentDto(respondent))
            .ToList();
    }
    
    public Respondent? GetRespondent(int respondentId)
    {
        return Respondents
            .Include(respondent => respondent.RespondentAnswers!)
                .ThenInclude(respondentAnswer => respondentAnswer.Question)
            .Include(respondent => respondent.RespondentAnswers!)
                .ThenInclude(respondentAnswer => respondentAnswer.Answer)
            .SingleOrDefault(respondent => respondent.Id == respondentId);
    }
    
    public List<RespondentAnswerDto> GetRespondentAnswerDtos(Respondent respondent)
    {
        return RespondentsAnswers
            .Include(respondentAnswer => respondentAnswer.Respondent)
            .Include(respondentAnswer => respondentAnswer.Question)
            .Include(respondentAnswer => respondentAnswer.Answer)
            .Where(respondentAnswer => respondentAnswer.Respondent!.Id == respondent.Id)
            .Select(respondentAnswer => new RespondentAnswerDto(respondentAnswer))
            .ToList();
    }
    
    public ActionResult<IEnumerable<QuestionnaireDto>> GetQuestionnaireDtos()
    {
        return Questionnaires
            .Include(questionnaire => questionnaire.Questions)!
            .ThenInclude(question => question.Answers)
            .Select(questionnaire => new QuestionnaireDto(questionnaire))
            .ToList();
    }
    
    public Question? GetQuestion(int? questionId)
    {
        return Questions
            .Include(question => question.Answers!)
            .SingleOrDefault(question => question.Id == questionId);
    }
    
    public int? GetMaxQuestionnaireScore()
    {
        return Questions
            .Include(question => question.Answers)
            .Sum(question => question.Answers!.Sum(answer => answer.Score));
    }

    public int? GetQuestionScoreSum(Question? question)
    {
        return Answers
            .Include(answer => answer.Question)
            .Where(answer => answer.Question!.Id == question!.Id)
            .Sum(answer => answer.Score);
    }
    
    public double? GetRespondentScore(Respondent respondent, List<Question> processedQuestions)
    {
        return respondent.RespondentAnswers!.Sum(respondentAnswer =>
        {
            var question = respondentAnswer.Question;

            if (processedQuestions.Contains(question!)) return 0; 

            var totalRespondentAnswers =
                respondent.RespondentAnswers!.Count(answer => answer.Question!.Id == question!.Id);

            if (totalRespondentAnswers == 1)
            {
                processedQuestions.Add(question!);
                var questionScoreSum = GetQuestionScoreSum(question);
                return questionScoreSum  * 0.5;
            }
            else
            {
                return respondentAnswer.Answer!.Score;
            }
        });
    }
    
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

        modelBuilder.Entity<Respondent>(builder =>
        {
            builder.HasKey(respondent => respondent.Id);
            builder.Property(respondent => respondent.Name);
        });

        modelBuilder.Entity<RespondentAnswer>(builder =>
        {
            builder.HasKey(respondentAnswer => respondentAnswer.Id);
            builder
                .HasOne(respondentAnswer => respondentAnswer.Respondent)
                .WithMany(respondent => respondent.RespondentAnswers);
            builder
                .HasOne(respondentAnswer => respondentAnswer.Question)
                .WithMany(question => question.RespondentAnswers);
            builder
                .HasOne(respondentAnswer => respondentAnswer.Answer)
                .WithMany(answer => answer.RespondentAnswers);
        });
    }
}