namespace QuestionnaireService.Models;

public class Question
{
    public int? Id { get; set; }
    public string? Text { get; set; }
    public virtual ICollection<Answer>? Answers { get; set; }
    public virtual Questionnaire? Questionnaire { get; set; }
    public ICollection<RespondentAnswer>? RespondentAnswers { get; set; }
}