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
using Bean_Mind.API.Payload.Request.QuestionAnswers;
using Bean_Mind_Data.Enums;
using Newtonsoft.Json;
using Bean_Mind.API.Service.Implement;
using Bean_Mind.API.Payload.Response.QuestionAnswers;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(Roles = "Teacher")]
        [HttpPost(ApiEndPointConstant.Question.Create)]
        [ProducesResponseType(typeof(CreateNewQuestionResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateQuestion([FromForm] CreateNewQuestionRequest request, [FromQuery] Guid questionLevelId)
        {
            var answers = JsonConvert.DeserializeObject<List<CreateNewQuestionAnswerRequest>>(request.Answers);
            CreateNewQuestionResponse response = await _questionService.CreateNewQuestion(request.Image, request.Text, request.OrderIndex, answers, request.QuestionType,questionLevelId);
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

        [Authorize(Roles = "Teacher")]
        [HttpDelete(ApiEndPointConstant.Question.DeleteQuestion)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteWorkSheetTemplate([FromRoute] Guid id)
        {
            var response = await _questionService.RemoveQuestion(id);

            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.Question.UpdateQuestion)]
        [ProducesResponseType(typeof(UpdateQuestionResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateQuestionDetails([FromRoute] Guid id, [FromForm] UpdateQuestionRequest request, [FromQuery] Guid questionLevelId)
        {
            // Cập nhật chi tiết câu hỏi
            var updatedQuestion = await _questionService.UpdateQuestionDetails(id, request.Image, request.Text, request.OrderIndex, request.QuestionType, questionLevelId);

            if (updatedQuestion == null)
            {
                return Problem(MessageConstant.QuestionMessage.UpdateQuestionFailed);
            }

            // Chuyển đổi thành UpdateQuestionResponse (hoặc kiểu phản hồi tương ứng của bạn)
            var response = new UpdateQuestionResponse
            {
                Id = updatedQuestion.Id,
                Text = updatedQuestion.Text,
                Image = updatedQuestion.Image,
                OrderIndex = updatedQuestion.OrderIndex,
                QuestionType = updatedQuestion.QuestionType,
                QuestionLevelId = updatedQuestion.QuestionLevelId,
                SchoolId = updatedQuestion.SchoolId,
               
            };

            return Ok(response);
        }
        [HttpPut(ApiEndPointConstant.Question.UpdateAnswers)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateQuestionAnswers([FromRoute] Guid id, [FromBody] List<UpdateQuestionAnswerRequest> answerRequests)
        {
            // Cập nhật câu trả lời của câu hỏi
            await _questionService.UpdateQuestionAnswers(id, answerRequests);

            return Ok(); // Trả về thành công nếu không có lỗi xảy ra
        }





    }
}
