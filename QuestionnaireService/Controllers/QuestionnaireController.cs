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

        var processedQuestions = new List<Question>();
        var respondentScore = _context.GetRespondentScore(respondent, processedQuestions);

        var maxQuestionnaireScore = _context.GetMaxQuestionnaireScore();

        var normalizedScore = respondentScore * 10 / maxQuestionnaireScore;

        return new QuestionnairePostDto
        {
            NormalizedScore = (int?)normalizedScore,
            Score = (int?)respondentScore
        };
    }

    [HttpPut]
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
        
        SeedQuestion1Data(questionnaire);
        SeedQuestion2Data(questionnaire);
        SeedQuestion3Data(questionnaire);
        SeedQuestion4Data(questionnaire);
        SeedQuestion5Data(questionnaire);
        
        var respondent1 = new Respondent
        {
            Name = "Eido"
        };

        _context.Respondents.Add(respondent1);
        
        _context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent1,
            Question = _context.Questions.ToArray()[0],
            Answer = _context.Questions.ToArray()[0].Answers!.ToList()[0]
        });
        
        _context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent1,
            Question = _context.Questions.ToArray()[1],
            Answer = _context.Questions.ToArray()[1].Answers.ToList()[0]
        });
        
        var respondent2 = new Respondent
        {
            Name = "Eve"
        };

        _context.Respondents.Add(respondent2);
        
        _context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent2,
            Question = _context.Questions.ToArray()[0],
            Answer = _context.Questions.ToArray()[0].Answers.ToList()[1]
        });
        
        _context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent2,
            Question = _context.Questions.ToArray()[0],
            Answer = _context.Questions.ToArray()[0].Answers.ToList()[2]
        });
        
        _context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent2,
            Question = _context.Questions.ToArray()[1],
            Answer = _context.Questions.ToArray()[1].Answers.ToList()[1]
        });
        
        _context.RespondentsAnswers.Add(new RespondentAnswer
        {
            Respondent = respondent2,
            Question = _context.Questions.ToArray()[1],
            Answer = _context.Questions.ToArray()[1].Answers.ToList()[2]
        });
            
        _context.SaveChanges();
    }

    private void SeedQuestion1Data(Questionnaire questionnaire)
    {
        var question = new Question
        {
            Text = "What does Cyber Quant aim to measure in an organization's cybersecurity?",
            Comment = "",
            Questionnaire = questionnaire
        };
        
        _context.Questions.Add(question);
        
        _context.Answers.Add(new Answer
        {
            Question = question,
            Text = "Impact of new cybersecurity controls",
            Score = 95
        });
            
        _context.Answers.Add(new Answer
        {
            Question = question,
            Text = "Financial risk of security breaches",
            Score = 25
        });
            
        _context.Answers.Add(new Answer
        {
            Question = question,
            Text = "How many coffee cups the IT team consumes",
            Score = 10
        });
        
        _context.SaveChanges();
    }
    
    private void SeedQuestion2Data(Questionnaire questionnaire)
    {
        var question = new Question
        {
            Text = "In the case study, how much did the bank decrease their financial risk by using Cyber Quant?",
            Questionnaire = questionnaire
        };
        
        _context.Questions.Add(question);
        
        _context.Answers.Add(new Answer
        {
            Question = question,
            Text = "$5",
            Score = 0
        });
            
        _context.Answers.Add(new Answer
        {
            Question = question,
            Text = "$100 million",
            Score = 80
        });
            
        _context.Answers.Add(new Answer
        {
            Question = question,
            Text = "$155 million",
            Score = 90
        });
        
        _context.SaveChanges();
    }
    
    private void SeedQuestion3Data(Questionnaire questionnaire)
    {
        var question = new Question
        {
            Text = "What is the goal of Cyber Quant's good user validation solution?",
            Questionnaire = questionnaire
        };
        
        _context.Questions.Add(question);
        
        _context.Answers.Add(new Answer
        {
            Question = question,
            Text = "Protecting online environments from account hacking",
            Score = 90
        });
            
        _context.Answers.Add(new Answer
        {
            Question = question,
            Text = "Assessing third-party risks",
            Score = 10
        });
            
        _context.Answers.Add(new Answer
        {
            Question = question,
            Text = "Prioritizing cybersecurity investments",
            Score = 25
        });
        
        _context.SaveChanges();
    }
    
    private void SeedQuestion4Data(Questionnaire questionnaire)
    {
        var question = new Question
        {
            Text = "How long does a typical Cyber Quant consulting engagement last?",
            Questionnaire = questionnaire
        };
        
        _context.Questions.Add(question);
        
        _context.Answers.Add(new Answer
        {
            Question = question,
            Text = "1 hour",
            Score = 0
        });
            
        _context.Answers.Add(new Answer
        {
            Question = question,
            Text = "1 week",
            Score = 30
        });

        _context.Answers.Add(new Answer
        {
            Question = question,
            Text = "3 weeks",
            Score = 80
        });
        
        _context.SaveChanges();
    }
    
    private void SeedQuestion5Data(Questionnaire questionnaire)
    {
        var question = new Question
        {
            Text = "How many cybersecurity controls does Cyber Quant assess?",
            Questionnaire = questionnaire
        };
        
        _context.Questions.Add(question);
        
        _context.Answers.Add(new Answer
        {
            Question = question,
            Text = "70",
            Score = 30
        });
            
        _context.Answers.Add(new Answer
        {
            Question = question,
            Text = "50",
            Score = 100
        });
            
        _context.Answers.Add(new Answer
        {
            Question = question,
            Text = "It depends whether the customer is liken by us",
            Score = 5
        });
        
        _context.SaveChanges();
    }
}