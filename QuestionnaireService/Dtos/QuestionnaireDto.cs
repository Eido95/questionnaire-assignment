using QuestionnaireService.Models;

namespace QuestionnaireService.Dtos;

public class QuestionnaireDto
{
    public int? Id { get; set; }
    public string? Subject { get; set; }
    public virtual ICollection<QuestionDto>? Questions { get; set; }
    
    public QuestionnaireDto(Questionnaire questionnaire)
    {
        Id = questionnaire.Id;
        Subject = questionnaire.Subject;
        if (questionnaire.Questions != null) Questions = questionnaire.Questions.Select(question => new QuestionDto(question)).ToList();
    }
}