using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Questions;
using Bean_Mind.API.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Bean_Mind.API.Payload.Response.Question;
using Bean_Mind.API.Payload.Response.Questions;
using Bean_Mind_Data.Models;

namespace Bean_Mind.API.Controllers
{
    [ApiController]
    public class QuestionController : BaseController<QuestionController>
    {
        private readonly IQuestionService _questionService;

        public QuestionController(ILogger<QuestionController> logger, IQuestionService questionService) : base(logger)
        {
            _questionService = questionService;
        }

        [HttpPost(ApiEndPointConstant.Question.Create)]
        [ProducesResponseType(typeof(CreateNewQuestionResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateNewQuestionRequest createNewQuestionRequest, [FromQuery] Guid questionLevelId)
        {
            CreateNewQuestionResponse response = await _questionService.CreateNewQuestion(createNewQuestionRequest,questionLevelId);
            if (response == null)
            {
                return Problem(MessageConstant.QuestionMessage.CreateQuestionFailed);
            }

            return CreatedAtAction(nameof(CreateQuestion), response);
        }
        [HttpGet(ApiEndPointConstant.Question.GetAll)]
        [ProducesResponseType(typeof(List<GetQuestionResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetAllQuestions([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var questions = await _questionService.GetAllQuestion(pageNumber, pageSize);
            if (questions == null || questions.Count == 0)
            {
                return Problem(MessageConstant.QuestionMessage.NoQuestionFound);
            }

            return Ok(questions);
        }
        [HttpGet(ApiEndPointConstant.Question.GetAnswerInQuestion)]
        [ProducesResponseType(typeof(List<QuestionAnswer>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetQuestionAnswersByQuestionId([FromRoute] Guid id)
        {
            var questionAnswers = await _questionService.GetQuestionAnswersByQuestionId(id);
            if (questionAnswers == null)
            {
                return Problem(MessageConstant.QuestionMessage.NotAnswerForQuestion);
            }
            return Ok(questionAnswers);
        }


    }
}
