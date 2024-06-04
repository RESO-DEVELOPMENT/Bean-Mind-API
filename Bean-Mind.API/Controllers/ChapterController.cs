using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Chapters;
using Bean_Mind.API.Payload.Response.Chapters;
using Bean_Mind.API.Payload.Response.Topics;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
{
    [ApiController]
    public class ChapterController : BaseController<ChapterController>
    {
        private readonly IChapterService _chapterService;
        public ChapterController(ILogger<ChapterController> logger, IChapterService chapterService) : base(logger)
        {
            _chapterService = chapterService;
        }

        [HttpPost(ApiEndPointConstant.Chapter.Create)]
        [ProducesResponseType(typeof(CreateNewChapterResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateChapter([FromBody] CreateNewChapterRequest createNewChapterRequest, [FromQuery] Guid subjectId)
        {
            CreateNewChapterResponse response = await _chapterService.CreateNewChapter(createNewChapterRequest, subjectId);
            if (response == null)
            {
                return Problem(MessageConstant.ChapterMessage.CreateNewChapterFailedMessage);
            }

            return CreatedAtAction(nameof(CreateChapter), response);
        }

        [HttpGet(ApiEndPointConstant.Chapter.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetChapterResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListChapter([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _chapterService.GetListChapter(pageNumber, pageSize);
            if (response == null)
            {
                return Problem(MessageConstant.ChapterMessage.ChaptersIsEmpty);
            }
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Chapter.GetById)]
        [ProducesResponseType(typeof(GetChapterResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetChapter([FromRoute] Guid id)
        {
            var response = await _chapterService.GetChapterById(id);

            return Ok(response);
        }

        [HttpDelete(ApiEndPointConstant.Chapter.DeleteChapter)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteChapter([FromRoute] Guid id)
        {

            var response = await _chapterService.RemoveChapter(id);

            return Ok(response);

        }

        [HttpPatch(ApiEndPointConstant.Chapter.UpdateChapter)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateChapter([FromBody] UpdateChapterRequest request, [FromRoute] Guid id, [FromQuery] Guid subjectId)
        {

            var response = await _chapterService.UpdateChapter(id, request, subjectId);
            if (response == false)
            {
                return Problem(MessageConstant.ChapterMessage.UpdateChapterFailedMessage);
            }

            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Chapter.GetTopicInChapter)]
        [ProducesResponseType(typeof(IPaginate<GetTopicResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetTopicInChaper([FromRoute] Guid id, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _chapterService.GetListTopic(id, pageNumber, pageSize);

            return Ok(response);
        }
    }
}
