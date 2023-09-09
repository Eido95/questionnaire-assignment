using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestionnaireService.DbContexts;
using QuestionnaireService.Dtos;
using QuestionnaireService.Models;

namespace QuestionnaireService.Controllers;

[ApiController]
[Route("api/v1/questionnaire/[controller]")]
public class RespondentAnswerController : ControllerBase
{
    private readonly QuestionnaireDbContext _context;

    public RespondentAnswerController(QuestionnaireDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(RespondentAnswerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<RespondentAnswerDto>> GetRespondentAnswers()
    {
        return _context.RespondentsAnswers
            .Include(respondentAnswer => respondentAnswer.Respondent)
            .Include(respondentAnswer => respondentAnswer.Question)
            .Include(respondentAnswer => respondentAnswer.Answer)
            .Select(respondentAnswer => new RespondentAnswerDto(respondentAnswer))
            .ToList();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<RespondentAnswerDto> GetRespondentAnswers(RespondentAnswerDto respondentAnswerDto)
    {
        var respondent = _context.Respondents.Find(respondentAnswerDto.RespondentId);
        var question = _context.Questions.Find(respondentAnswerDto.QuestionId);
        var answer = _context.Answers.Find(respondentAnswerDto.AnswerId);

        if (respondent == null || question == null || answer == null)
        {
            return NotFound();
        }
        
        _context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent,
            Question = question,
            Answer = answer
        });

        _context.SaveChanges();

        return Ok();
    }
}