using QuestionnaireService.Models;

namespace QuestionnaireService.Dtos;

public class RespondentDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }

    public RespondentDto(Respondent respondent)
    {
        Id = respondent.Id;
        Name = respondent.Name;
    }
}