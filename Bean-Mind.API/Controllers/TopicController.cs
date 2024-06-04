using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Topics;
using Bean_Mind.API.Payload.Response.Topics;
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
                return Problem(MessageConstant.TopicMessage.CreateNewTopicFailedMessage);
            }
            return CreatedAtAction(nameof(CreateTopic), response);
        }

        [HttpGet(ApiEndPointConstant.Topic.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetTopicResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> getAll([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _topicService.GetListTopic(pageNumber, pageSize);
            if(response == null)
            {
                return Problem(MessageConstant.TopicMessage.ListIsEmpty);
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
        public async Task<IActionResult> UpdateTopic([FromRoute] Guid id, [FromQuery] Guid chapterId, UpdateTopicRequest request)
        {
            var response = await _topicService.UpdateTopic(id, chapterId, request);
            if(response == false)
            {
                return Problem(MessageConstant.TopicMessage.UpdateTopicFailedMessage);
            }
            return Ok(response);
        }
    }
}
