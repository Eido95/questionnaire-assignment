namespace QuestionnaireService.Models;

public class Respondent
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public ICollection<RespondentAnswer>? RespondentAnswers { get; set; }
}