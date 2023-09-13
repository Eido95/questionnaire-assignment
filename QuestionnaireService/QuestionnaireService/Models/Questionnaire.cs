namespace QuestionnaireService.Models;

public class Questionnaire
{
    public int? Id { get; set; }
    public string? Subject { get; set; }
    public virtual ICollection<Question>? Questions { get; set; }
}