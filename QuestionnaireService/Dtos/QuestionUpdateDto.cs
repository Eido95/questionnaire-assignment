namespace QuestionnaireService.Dtos;

public class QuestionUpdateDto
{
    public int? Id { get; set; }
    public virtual ICollection<int>? AnswerIds { get; set; }

    public QuestionUpdateDto() { }
}