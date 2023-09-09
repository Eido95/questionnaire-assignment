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
    
        [HttpPost]
    public void PostQuestionnaire()
    {
        SeedData();
    }
    
    private void SeedData()
    {
        // Creates the database if not exists
        _context.Database.EnsureCreated();

        var question1 = new Question
        {
            Text = "password policy – pass length:",
            Comment = "I'm a comment!",
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