using QuestionnaireService.Models;

namespace QuestionnaireService.Dtos;

public class AnswerDto
{
    public int? Id { get; set; }
    public string? Text { get; set; }

    public AnswerDto() { }
    public AnswerDto(Answer answer)
    {
        Id = answer.Id;
        Text = answer.Text;
    }
}