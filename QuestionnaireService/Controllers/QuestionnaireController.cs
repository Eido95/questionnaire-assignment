using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestionnaireService.DbContexts;
using QuestionnaireService.Dtos;
using QuestionnaireService.Models;

namespace QuestionnaireService.Controllers;

[ApiController]
[Route("api/v1/questionnaire/[controller]")]
public class QuestionnaireController : ControllerBase
{
    private readonly QuestionnaireDbContext _context;

    public QuestionnaireController(QuestionnaireDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(QuestionnaireDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<QuestionnaireDto>> GetQuestionnaires()
    {
        return _context.GetQuestionnaireDtos();
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<QuestionnairePostDto> PostQuestionnaire(int respondentId)
    {
        var respondent = _context.GetRespondent(respondentId);

        if (respondent == null)
        {
            return NotFound();
        }

        var respondentScore = _context.GetRespondentScore(respondent);

        var maxQuestionnaireScore = _context.GetMaxQuestionnaireScore();

        var normalizedScore = respondentScore * 10 / maxQuestionnaireScore;

        return new QuestionnairePostDto
        {
            NormalizedScore = (int?)normalizedScore,
            Score = (int?)respondentScore
        };
    }
}