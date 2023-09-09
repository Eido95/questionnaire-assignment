using QuestionnaireService.Models;

namespace QuestionnaireService.Dtos;

public class RespondentAnswerDto
{
    public int? Id { get; set; }
    public int? RespondentId { get; set; }
    public int? QuestionId { get; set; }
    public int? AnswerId { get; set; }

    public RespondentAnswerDto(RespondentAnswer respondentAnswer)
    {
        Id = respondentAnswer.Id;
        if (respondentAnswer.Respondent != null) RespondentId = respondentAnswer.Respondent.Id;
        if (respondentAnswer.Question != null) QuestionId = respondentAnswer.Question.Id;
        if (respondentAnswer.Answer != null) AnswerId = respondentAnswer.Answer.Id;
    }
}