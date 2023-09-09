namespace QuestionnaireService.Models;

public class RespondentAnswer
{
    public int? Id { get; set; }
    public virtual Respondent? Respondent { get; set; }
    public virtual Question? Question { get; set; }
    public virtual Answer? Answer { get; set; }
}