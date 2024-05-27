
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Topics;
using Bean_Mind.API.Payload.Response.Students;
using Bean_Mind.API.Payload.Response.Topics;
using Bean_Mind.API.Service.Implement;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
{
    [ApiController]
    public class TopicController : BaseController<TopicController>
    {
        private readonly ITopicService _topicService;
        public TopicController(ILogger<TopicController> logger, ITopicService topicService) : base(logger)
        {
            _topicService = topicService;
        }

        [HttpPost(ApiEndPointConstant.Topic.Create)]
        [ProducesResponseType(typeof(CreateNewTopicResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateTopic([FromBody] CreateNewTopicRequest createNewTopicRequest, [FromQuery] Guid chatterId)
        {
            CreateNewTopicResponse response = await _topicService.CreateNewTopic(createNewTopicRequest, chatterId);
            if (response == null)
            {
                return Problem(MessageConstant.Topic.CreateNewTopicFailedMessage);
            }
            return CreatedAtAction(nameof(CreateTopic), response);
        }

        [HttpGet(ApiEndPointConstant.Topic.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetTopicResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> getAll([FromQuery] int page, [FromQuery] int size)
        {
            var response = await _topicService.GetListTopic(page, size);
            if(response == null)
            {
                return Problem(MessageConstant.Topic.ListIsEmpty);
            }
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Topic.GetById)]
        [ProducesResponseType(typeof(GetTopicResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetTopic([FromRoute] Guid id)
        {
            var response = await _topicService.GetTopicById(id);
            return Ok(response);
        }

        [HttpDelete(ApiEndPointConstant.Topic.DeleteTopic)]
        [ProducesResponseType(typeof(Boolean), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteTopic([FromRoute] Guid id)
        {
            var response = await _topicService.DeleteTopic(id);
            return Ok(response);
        }

        [HttpPatch(ApiEndPointConstant.Topic.UpdateTopic)]
        [ProducesResponseType(typeof(Boolean), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateTopic([FromRoute] Guid topicId, [FromQuery] Guid chapterId, UpdateTopicRequest request)
        {
            var response = await _topicService.UpdateTopic(topicId, chapterId, request);
            if(response == false)
            {
                return Problem(MessageConstant.Topic.UpdateTopicFailedMessage);
            }
            return Ok(response);
        }
    }
}
