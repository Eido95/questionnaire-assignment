using QuestionnaireService.Models;

namespace QuestionnaireService.Dtos;

public class QuestionDto
{
    public int? Id { get; set; }
    public string? Text { get; set; }
    public string? Comment { get; set; }
    public virtual ICollection<AnswerDto>? Answers { get; set; }

    public QuestionDto() { }
    public QuestionDto(Question question)
    {
        Id = question.Id;
        Text = question.Text;
        Comment = question.GetType() == typeof(MultipleChoiceQuestion) ? ((MultipleChoiceQuestion)question).Comment : null;
        if (question.Answers != null) Answers = question.Answers.Select(answer => new AnswerDto(answer)).ToList();
    }
}