using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Response.StudentInCourse;
using Bean_Mind.API.Service.Interface;
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
            return CreatedAtAction(nameof(CreateNewStudentInCourseResponse), response);
        }
    }
}
