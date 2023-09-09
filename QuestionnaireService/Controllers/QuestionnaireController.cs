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
        return _context.Questionnaires
            .Include(questionnaire => questionnaire.Questions)!
            .ThenInclude(question => question.Answers)
            .Select(questionnaire => new QuestionnaireDto(questionnaire))
            .ToList();
    }
    
    [HttpPost]
    public void PostQuestionnaire()
    {
        SeedData();
    }
    
    private void SeedData()
    {
        // Creates the database if not exists
        _context.Database.EnsureCreated();

        var questionnaire = new Questionnaire
        {
            Subject = "Dummy"
        };

        _context.Questionnaires.Add(questionnaire);

        var question1 = new Question
        {
            Text = "password policy – pass length:",
            Comment = "I'm a comment!",
            Questionnaire = questionnaire
        };
        
        _context.Questions.Add(question1);
        
        _context.Answers.Add(new Answer
        {
            Question = question1,
            Text = "4",
            Score = 0
        });
            
        _context.Answers.Add(new Answer
        {
            Question = question1,
            Text = "6-8",
            Score = 50
        });
            
        _context.Answers.Add(new Answer
        {
            Question = question1,
            Text = "12+",
            Score = 90
        });
            
        _context.SaveChanges();
    }
}