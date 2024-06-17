using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.QuestionLevels;
using Bean_Mind.API.Payload.Request.WorkSheets;
using Bean_Mind.API.Payload.Response.QuestionLevels;
using Bean_Mind.API.Payload.Response.WorkSheets;
using Bean_Mind.API.Service.Implement;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
{
    public class QuestionLevelController : BaseController<QuestionLevelController>
    {
        private readonly IQuestionLevelService _questionLevelService;
        public QuestionLevelController(ILogger<QuestionLevelController> logger, IQuestionLevelService questionLevelService) : base(logger)
        {
            _questionLevelService = questionLevelService;
        }

        [HttpPost(ApiEndPointConstant.QuestionLevel.Create)]
        [ProducesResponseType(typeof(CreateNewQuestionLevelResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateNewQuestionLevel([FromBody] CreateNewQuestionLevelRequest request)
        {
            CreateNewQuestionLevelResponse response = await _questionLevelService.CreateNewQuestionLevel(request);
            if (response == null)
            {
                return Problem(MessageConstant.QuestionLevelMessage.CreateNewQuestionLevelFailedMessage);
            }
            return CreatedAtAction(nameof(CreateNewQuestionLevel), response);
        }

        [HttpGet(ApiEndPointConstant.QuestionLevel.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetQuestionLevelResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListQuestionLevel([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _questionLevelService.GetListQuestionLevel(pageNumber, pageSize);
            if (response == null)
            {
                return Problem(MessageConstant.QuestionLevelMessage.QuestionLevelIsEmpty);
            }
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.QuestionLevel.GetById)]
        [ProducesResponseType(typeof(GetQuestionLevelResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetQuestionLevelById([FromRoute] Guid id)
        {
            var response = await _questionLevelService.GetQuestionLevelById(id);
            return Ok(response);
        }

        [HttpDelete(ApiEndPointConstant.QuestionLevel.DeleteQuestionLevel)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteQuestionLevel([FromRoute] Guid id)
        {
            var response = await _questionLevelService.RemoveQuestionLevel(id);
            return Ok(response);
        }

        [HttpPatch(ApiEndPointConstant.QuestionLevel.UpdateQuestionLevel)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateQuestionLevel([FromRoute] Guid id, [FromBody] UpdateQuestionLevelRequest request)
        {

            var response = await _questionLevelService.UpdateQuestionLevel(id, request);
            if (response == false)
            {
                return Problem(MessageConstant.QuestionLevelMessage.UpdateQuestionLevelFailedMessage);
            }

            return Ok(response);
        }
    }
}
