using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Videos;
using Bean_Mind.API.Payload.Response.Videos;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
{
    public class VideoController : BaseController<VideoController>
    {
        private readonly IVideoService _videoService;

        public VideoController(ILogger<VideoController> logger, IVideoService videoService) : base(logger)
        {
            _videoService = videoService;
        }

        [HttpPost(ApiEndPointConstant.Video.Create)]
        [ProducesResponseType(typeof(CreateNewVideoResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateVideo([FromForm] CreateNewVideoRequest createNewVideoRequest, [FromQuery] Guid activityId)
        {
            CreateNewVideoResponse response = await _videoService.CreateNewVideo(createNewVideoRequest, activityId);
            if (response == null)
            {
                return Problem(MessageConstant.VideoMessage.CreateNewVideoFailedMessage);
            }
            return CreatedAtAction(nameof(CreateVideo), response);
        }

        [HttpGet(ApiEndPointConstant.Video.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetVideoResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListVideo([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _videoService.GetListVideo(pageNumber, pageSize);
            if (response == null)
            {
                return Problem(MessageConstant.VideoMessage.VideoIsEmpty);
            }
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Video.GetById)]
        [ProducesResponseType(typeof(GetVideoResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetVideo([FromRoute] Guid id)
        {
            var response = await _videoService.GetVideoById(id);

            return Ok(response);
        }

        [HttpDelete(ApiEndPointConstant.Video.DeleteVideo)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteVideo([FromRoute] Guid id)
        {
            var response = await _videoService.DeleteVideo(id);

            return Ok(response);
        }

        [HttpPatch(ApiEndPointConstant.Video.UpdateVideo)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateVideo([FromForm] UpdateVideoRequest request, [FromRoute] Guid id, [FromQuery] Guid activityId)
        {

            var response = await _videoService.UpdateVideo(id, activityId, request);
            if (response == false)
            {
                return Problem(MessageConstant.VideoMessage.UpdateVideoFailedMessage);
            }

            return Ok(response);
        }
    }
}
