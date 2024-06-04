using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Students;
using Bean_Mind.API.Payload.Response.Students;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
{
    [ApiController]
    public class StudentController : BaseController<StudentController>
    {
        private readonly IStudentService _studentService;

        public StudentController(ILogger<StudentController> logger, IStudentService studentService) : base(logger)
        {
            _studentService = studentService;
        }

        [HttpPost(ApiEndPointConstant.Student.Create)]
        [ProducesResponseType(typeof(CreateNewStudentResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateStudent([FromBody] CreateNewStudentRequest createNewStudentRequest, [FromQuery] Guid schoolId, [FromQuery] Guid parentId)
        {
            CreateNewStudentResponse response = await _studentService.CreateNewStudent(createNewStudentRequest, schoolId, parentId);
            if (response == null)
            {
                return Problem(MessageConstant.StudentMessage.CreateNewStudentFailedMessage);
            }

            return CreatedAtAction(nameof(CreateStudent), response);
        }

            [HttpGet(ApiEndPointConstant.Student.GetAll)]
            [ProducesResponseType(typeof(IPaginate<GetStudentResponse>), StatusCodes.Status200OK)]
            [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListStudent([FromQuery] int? page, [FromQuery] int? size) {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _studentService.getListStudent(pageNumber, pageSize);
            if(response == null)
            {
                return Problem(MessageConstant.StudentMessage.StudentsIsEmpty);
            }

            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Student.GetById)]
        [ProducesResponseType(typeof(GetStudentResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetStudent([FromRoute] Guid id)
        {
            var response = await _studentService.getStudentById(id);
            
            return Ok(response);
        }

        [HttpDelete(ApiEndPointConstant.Student.DeleteStudent)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteStudent([FromRoute] Guid id)
        {
            var response = await _studentService.RemoveStudent(id);
            return Ok(response);
        }

        [HttpPatch(ApiEndPointConstant.Student.UpdateStudent)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateStudent([FromRoute] Guid id, [FromBody] UpdateStudentRequest request, [FromQuery] Guid schoolId, [FromQuery] Guid parentId)
        {
            var response = await _studentService.UpdateStudent(id, request, schoolId, parentId);
            return Ok(response);
        }
    }
}
