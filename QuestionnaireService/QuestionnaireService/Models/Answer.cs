namespace QuestionnaireService.Models;

public class Answer
{
    public int? Id { get; set; }
    public string? Text { get; set; }
    public int? Score { get; set; }
    public virtual Question? Question { get; set; }
    public ICollection<RespondentAnswer>? RespondentAnswers { get; set; }
}