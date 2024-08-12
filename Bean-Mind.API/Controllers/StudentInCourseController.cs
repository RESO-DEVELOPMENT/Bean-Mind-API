using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Students;
using Bean_Mind.API.Payload.Response.StudentInCourse;
using Bean_Mind.API.Payload.Response.Students;
using Bean_Mind.API.Service.Implement;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
{
    public class StudentInCourseController : BaseController<StudentInCourseController>
    {
        private readonly IStudentInCourseService _studentInCourseService;
        public StudentInCourseController(ILogger<StudentInCourseController> logger, IStudentInCourseService studentInCourseService) : base(logger)
        {
            _studentInCourseService = studentInCourseService;
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost(ApiEndPointConstant.StudentInCourse.Create)]
        [ProducesResponseType(typeof(CreateNewStudentInCourseResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateStudentInCourse([FromQuery] Guid studentId, [FromQuery] List<Guid> courseIds)
        {
            CreateNewStudentInCourseResponse response = await _studentInCourseService.CreateStudentInCourse(studentId, courseIds);
            if (response == null)
            {
                return Problem(MessageConstant.StudentInCourseMessage.CreateFailed);
            }
            return CreatedAtAction(nameof(CreateStudentInCourse), response);
        }

        [HttpGet(ApiEndPointConstant.StudentInCourse.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetStudentInCourseResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListStudentInCourse([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _studentInCourseService.GetAllStudentInCourses(pageNumber, pageSize);
            if (response == null)
            {
                return Problem(MessageConstant.StudentInCourseMessage.IsEmpty);
            }

            return Ok(response);
        }

        [Authorize(Roles = "Teacher")]
        [HttpDelete(ApiEndPointConstant.StudentInCourse.DeleteStudentInCourse)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteStudent([FromRoute] Guid id)
        {
            var response = await _studentInCourseService.DeleteStudentInCourse(id);
            return Ok(response);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPatch(ApiEndPointConstant.StudentInCourse.UpdateStudentInCourse)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateStudent([FromRoute] Guid id, [FromQuery] Guid studentId, [FromQuery] Guid courseId)
        {
            var response = await _studentInCourseService.UpdateStudentInCourse(id, studentId, courseId);
            if (response == false)
                return Problem(MessageConstant.StudentInCourseMessage.UpdateFailed);
            return Ok(response);
        }
    }
}
