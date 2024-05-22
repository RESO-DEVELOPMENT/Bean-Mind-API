﻿using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.School;
using Bean_Mind.API.Payload.Request.Student;
using Bean_Mind.API.Payload.Response.Student;
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
                return Problem(MessageConstant.School.CreateNewSchoolFailedMessage);
            }

            return CreatedAtAction(nameof(CreateStudent), response);
        }

        [HttpGet(ApiEndPointConstant.Student.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetStudentResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListStudent([FromQuery] int page, [FromQuery] int size) { 
            var response = await _studentService.getListStudent(page, size);
            if(response == null)
            {
                return Problem(MessageConstant.Student.StudentsIsEmpty);
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
    }
}