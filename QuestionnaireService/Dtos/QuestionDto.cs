using QuestionnaireService.Models;

namespace QuestionnaireService.Dtos;

public class QuestionDto
{
    public int? Id { get; set; }
    public string? Text { get; set; }
    public bool IsSingleChoice { get; set; }
    public string? Comment { get; set; }
    public virtual ICollection<AnswerDto>? Answers { get; set; }

    public QuestionDto() { }
    public QuestionDto(Question question)
    {
        Id = question.Id;
        Text = question.Text;
        IsSingleChoice = question.IsSingleChoice;
        Comment = question.Comment;
        if (question.Answers != null) Answers = question.Answers.Select(answer => new AnswerDto(answer)).ToList();
    }
}