using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Schools;
using Bean_Mind.API.Payload.Request.Subjects;
using Bean_Mind.API.Payload.Response.Chapters;
using Bean_Mind.API.Payload.Response.Schools;
using Bean_Mind.API.Payload.Response.Students;
using Bean_Mind.API.Payload.Response.Subjects;
using Bean_Mind.API.Service.Implement;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
{
    [ApiController]
    public class SubjectController : BaseController<SubjectController>
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ILogger<SubjectController> logger, ISubjectService subjectService) : base(logger)
        {
            _subjectService = subjectService;
        }

        [HttpPost(ApiEndPointConstant.Subject.Create)]
        [ProducesResponseType(typeof(CreateNewSubjectResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateSubject([FromBody] CreateNewSubjectRequest createNewStudentRequest, [FromQuery] Guid courseId)
        {
            CreateNewSubjectResponse response = await _subjectService.CreateNewSubject(createNewStudentRequest, courseId);
            if (response == null)
            {
                return Problem(MessageConstant.SubjectMessage.CreateNewSubjectFailedMessage);
            }

            return CreatedAtAction(nameof(CreateSubject), response);
        }

        [HttpGet(ApiEndPointConstant.Subject.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetSubjectResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListSubject([FromQuery] int page, [FromQuery] int size)
        {
            var response = await _subjectService.getListSubject(page, size);
            if (response == null)
            {
                return Problem(MessageConstant.SubjectMessage.SubjectsIsEmpty);
            }
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Subject.GetById)]
        [ProducesResponseType(typeof(GetSubjectResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetSubject([FromRoute] Guid id)
        {
            var response = await _subjectService.getSubjectById(id);

            return Ok(response);
        }

        [HttpDelete(ApiEndPointConstant.Subject.DeleteSubject)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteSubject([FromRoute] Guid id)
        {

            var response = await _subjectService.RemoveSubject(id);

            return Ok(response);

        }

        [HttpPatch(ApiEndPointConstant.Subject.UpdateSubject)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateSubject([FromBody] UpdateSubjectRequest request, [FromRoute] Guid id, [FromQuery] Guid courseId)
        {

            var response = await _subjectService.UpdateSubject(id, request, courseId);
            if (response == false)
            {
                return Problem(MessageConstant.SubjectMessage.UpdateSubjectFailedMessage);
            }

            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Subject.GetChapterInSubject)]
        [ProducesResponseType(typeof(IPaginate<GetChapterResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetChapterInSubject([FromRoute] Guid id, [FromQuery] int page, [FromQuery] int size)
        {
            var response = await _subjectService.GetListChapters(id, page, size);

            return Ok(response);
        }
    }
}
