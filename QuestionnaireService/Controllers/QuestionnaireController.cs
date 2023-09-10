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
        
        var question2 = new Question
        {
            Text = "What is a Tuna?",
            Questionnaire = questionnaire
        };
        
        _context.Questions.Add(question2);
        
        _context.Answers.Add(new Answer
        {
            Question = question2,
            Text = "A bird",
            Score = 0
        });
            
        _context.Answers.Add(new Answer
        {
            Question = question2,
            Text = "A fish",
            Score = 50
        });
            
        _context.Answers.Add(new Answer
        {
            Question = question2,
            Text = "A saltwater fish",
            Score = 90
        });

        var respondent1 = new Respondent
        {
            Name = "Eido"
        };

        _context.Respondents.Add(respondent1);
        
        _context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent1,
            Question = question1,
            Answer = question1.Answers.ToList()[0]
        });
        
        _context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent1,
            Question = question2,
            Answer = question2.Answers.ToList()[0]
        });
        
        var respondent2 = new Respondent
        {
            Name = "Eve"
        };

        _context.Respondents.Add(respondent2);
        
        _context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent2,
            Question = question1,
            Answer = question1.Answers.ToList()[1]
        });
        
        _context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent2,
            Question = question1,
            Answer = question1.Answers.ToList()[2]
        });
        
        _context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent2,
            Question = question2,
            Answer = question2.Answers.ToList()[1]
        });
        
        _context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent2,
            Question = question2,
            Answer = question2.Answers.ToList()[2]
        });
            
        _context.SaveChanges();
    }
}