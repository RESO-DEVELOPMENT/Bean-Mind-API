using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Activities;
using Bean_Mind.API.Payload.Response.Activities;
using Bean_Mind.API.Payload.Response.Courses;
using Bean_Mind.API.Payload.Response.Documents;
using Bean_Mind.API.Service.Implement;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
{
    [ApiController]
    public class ActivityController : BaseController<ActivityController>
    {
        private readonly IActivityService _activityService;
        public ActivityController(ILogger<ActivityController> logger, IActivityService activityService) : base(logger)
        {
            _activityService = activityService;
        }

        [HttpPost(ApiEndPointConstant.Activity.Create)]
        [ProducesResponseType(typeof(CreateNewActivityResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateNewActivity([FromBody]CreateNewActivityRequest request, [FromQuery] Guid topicId)
        {
            CreateNewActivityResponse response = await _activityService.CreateNewActivity(request, topicId);
            if(response == null)
            {
                return Problem(MessageConstant.ActivityMessage.CreateNewActivityFailedMessage);
            }
            return CreatedAtAction(nameof(CreateNewActivity), response);

        }

        [HttpGet(ApiEndPointConstant.Activity.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetActivityResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof (ProblemDetails))]
        public async Task<IActionResult> GetAllActivity([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _activityService.GetListActivity(pageNumber, pageSize);
            if(response == null)
            {
                return Problem(MessageConstant.ActivityMessage.ActivityIsEmpty);
            }
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Activity.GetById)]
        [ProducesResponseType(typeof(GetActivityResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof (ProblemDetails))]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var response = await _activityService.GetActivityById(id);
            return Ok(response);

        }

        [HttpDelete(ApiEndPointConstant.Activity.DeleteActivity)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof (ProblemDetails))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = await _activityService.DeleteActivity(id);
            return Ok(response);
        }

        [HttpPatch(ApiEndPointConstant.Activity.UpdateActivity)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateActivityRequest request, [FromQuery] Guid topicId)
        {
            var response = await _activityService.UpdateActivitỵ(id, request, topicId);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Activity.GetDocumentInActivity)]
        [ProducesResponseType(typeof(IPaginate<GetDocumentResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetDocumentInActivity([FromRoute] Guid id, [FromQuery] int page, [FromQuery] int size)
        {
            var response = await _activityService.GetListDocument(id, page, size);

            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Activity.GetVideoInActivity)]
        [ProducesResponseType(typeof(IPaginate<GetDocumentResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetVideoInActivity([FromRoute] Guid id, [FromQuery] int page, [FromQuery] int size)
        {
            var response = await _activityService.GetListVideo(id, page, size);

            return Ok(response);
        }
    }
}
