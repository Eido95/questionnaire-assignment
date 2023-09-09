using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestionnaireService.DbContexts;
using QuestionnaireService.Dtos;
using QuestionnaireService.Models;

namespace QuestionnaireService.Controllers;

[ApiController]
[Route("api/v1/questionnaire/[controller]")]
public class QuestionController : ControllerBase
{
    private readonly QuestionnaireDbContext _context;

    public QuestionController(QuestionnaireDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(QuestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<QuestionDto>> GetQuestions()
    {
        return _context.Questions.Include(npp => npp.Answers)
            .Select(question => new QuestionDto(question))
            .ToList();
    }
}