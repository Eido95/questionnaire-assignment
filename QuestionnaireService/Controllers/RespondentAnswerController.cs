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
    public ActionResult<IEnumerable<RespondentAnswerDto>> GetRespondentAnswers(int respondentId)
    {
        var respondent = _context.GetRespondent(respondentId);

        if (respondent == null)
        {
            return NotFound();
        }
        
        return _context.GetRespondentAnswerDtos(respondent);
    }
}