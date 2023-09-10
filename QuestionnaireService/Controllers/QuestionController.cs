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
    public ActionResult UpdateQuestion(int respondentId, QuestionUpdateDto questionUpdateDto)
    {
        var respondent = _context.GetRespondent(respondentId);

        if (respondent == null)
        {
            return NotFound();
        }

        var question = _context.GetQuestion(questionUpdateDto.Id);

        if (question == null)
        {
            return BadRequest();
        }

        var areAnswersValid = questionUpdateDto.AnswerIds!
            .All(answerId => question.Answers!
                .Any(answer => answerId == answer.Id));

        if (!areAnswersValid)
        {
            return BadRequest();
        }
        
        var answers = _context.Answers
            .Where(answer => questionUpdateDto.AnswerIds!.Contains((int)answer.Id!));

        RemoveAllRespondentQuestionAnswers(question, respondent);

        UpdateRespondentQuestionAnswers(question, respondent, answers);
        
        return Ok();
    }

    private void UpdateRespondentQuestionAnswers(Question question, Respondent respondent, IEnumerable<Answer> answers)
    {
        var updatedRespondentAnswers = answers!.Select(answerDto => new RespondentAnswer
        {
            Answer = question.Answers!.SingleOrDefault(answer => answer.Id == answerDto.Id),
            Question = question,
            Respondent = respondent
        });

        _context.RespondentsAnswers.AddRange(updatedRespondentAnswers);
        
        _context.SaveChanges();
    }

    private void RemoveAllRespondentQuestionAnswers(Question question, Respondent respondent)
    {
        var respondentQuestionAnswers = respondent.RespondentAnswers!.Where(answer => answer.Question!.Id == question.Id);

        _context.RespondentsAnswers.RemoveRange(respondentQuestionAnswers);
    }
}