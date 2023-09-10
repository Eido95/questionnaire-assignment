using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestionnaireService.DbContexts;
using QuestionnaireService.Dtos;

namespace QuestionnaireService.Controllers;

[ApiController]
[Route("api/v1/questionnaire/[controller]")]
public class RespondentController : ControllerBase
{
    private readonly QuestionnaireDbContext _context;

    public RespondentController(QuestionnaireDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(RespondentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<RespondentDto>> GetRespondents()
    {
        return _context.GetRespondentsDtos();
    }
}