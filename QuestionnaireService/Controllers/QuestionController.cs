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

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult UpdateQuestion(int respondentId, QuestionDto questionDto)
    {
        var respondent = _context.Respondents
            .Include(respondent => respondent.RespondentAnswers!)
            .ThenInclude(respondentAnswer => respondentAnswer.Question)
            .SingleOrDefault(respondent => respondent.Id == respondentId);

        if (respondent == null)
        {
            return NotFound();
        }

        var question = _context.Questions
            .Include(question => question.Answers!).SingleOrDefault(question => question.Id == questionDto.Id);

        if (question == null)
        {
            return BadRequest();
        }

        var areAnswersValid = questionDto.Answers!.All(answerDto => question.Answers!.Any(answer => answerDto.Id == answer.Id));

        if (!areAnswersValid)
        {
            return BadRequest();
        }

        RemoveAllRespondentQuestionAnswers(respondent, question);

        UpdateRespondentQuestionAnswers(questionDto, question, respondent);
        
        return Ok();
    }

    private void UpdateRespondentQuestionAnswers(QuestionDto questionDto, Question question, Respondent respondent)
    {
        var updatedRespondentAnswers = questionDto.Answers!.Select(answerDto => new RespondentAnswer
        {
            Answer = question.Answers!.SingleOrDefault(answer => answer.Id == answerDto.Id),
            Question = question,
            Respondent = respondent
        });

        _context.RespondentsAnswers.AddRange(updatedRespondentAnswers);
        
        _context.SaveChanges();
    }

    private void RemoveAllRespondentQuestionAnswers(Respondent respondent, Question question)
    {
        var respondentQuestionAnswers = respondent.RespondentAnswers!.Where(answer => answer.Question!.Id == question.Id);

        _context.RespondentsAnswers.RemoveRange(respondentQuestionAnswers);
    }
}