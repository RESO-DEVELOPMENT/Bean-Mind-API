using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Schools;
using Bean_Mind.API.Payload.Request.Subjects;
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
        public async Task<IActionResult> CreateStudent([FromBody] CreateNewSubjectRequest createNewStudentRequest, [FromQuery] Guid courseId)
        {
            CreateNewSubjectResponse response = await _subjectService.CreateNewSubject(createNewStudentRequest, courseId);
            if (response == null)
            {
                return Problem(MessageConstant.SubjectMessage.CreateNewSubjectFailedMessage);
            }

            return CreatedAtAction(nameof(CreateStudent), response);
        }

        [HttpGet(ApiEndPointConstant.Subject.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetSubjectResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListStudent([FromQuery] int page, [FromQuery] int size)
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
        public async Task<IActionResult> GetStudent([FromRoute] Guid id)
        {
            var response = await _subjectService.getSubjectById(id);

            return Ok(response);
        }

        [HttpDelete(ApiEndPointConstant.Subject.DeleteSubject)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteSchool([FromRoute] Guid id)
        {

            var response = await _subjectService.RemoveSubject(id);

            return Ok(response);

        }

        [HttpPatch(ApiEndPointConstant.Subject.UpdateSubject)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateSchool([FromBody] UpdateSubjectRequest request, [FromRoute] Guid id, [FromQuery] Guid courseId)
        {

            var response = await _subjectService.UpdateSubject(id, request, courseId);
            if (response == false)
            {
                return Problem(MessageConstant.SubjectMessage.UpdateSubjectFailedMessage);
            }

            return Ok(response);
        }
    }
}
