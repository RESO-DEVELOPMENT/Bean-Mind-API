using Bean_Mind.API.Constants;
using Microsoft.AspNetCore.Mvc;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Payload.Request.Teachers;
using Bean_Mind.API.Payload.Response.Teachers;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using Bean_Mind.API.Payload;
using Bean_Mind.API.Utils;

namespace Bean_Mind.API.Controllers
{


    [Route(ApiEndPointConstant.Teacher.TeacherEndpoint)]
        [ApiController]
    public class TeacherController : BaseController<TeacherController>
    {
        private readonly ITeacherService _teacherService;


        public TeacherController(ILogger<TeacherController> logger, ITeacherService teacherService) : base(logger)
        {
            _teacherService = teacherService;
        }


        [HttpPost(ApiEndPointConstant.Teacher.Create)]
        [ProducesResponseType(typeof(CreateNewTeacherResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
        public async Task<IActionResult> CreateTeacher([FromBody] CreateNewTeacherResquest newTeacherRequest, [FromQuery] Guid schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = "Invalid input data",
                    TimeStamp = TimeUtils.GetCurrentSEATime()
                });
            }

            var response = await _teacherService.CreateTeacher(newTeacherRequest, schoolId);
            if (response == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = "Failed to create teacher",
                    TimeStamp = TimeUtils.GetCurrentSEATime()
                });
            }

            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Teacher.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetTeacherResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTeachers([FromQuery] int page, [FromQuery] int size)
        {
            var teachers = await _teacherService.GetAllTeachers(page, size);
            return Ok(teachers);
        }

        [HttpGet(ApiEndPointConstant.Teacher.GetById)]
        [ProducesResponseType(typeof(Teacher), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundObjectResult))]
        public async Task<IActionResult> GetTeacherById([FromRoute] Guid teacherId)
        {
            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null) 
            {
                return NotFound(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Error = "Teacher not found",
                    TimeStamp = TimeUtils.GetCurrentSEATime()
                });
            }

            return Ok(teacher);
        }

        [HttpDelete(ApiEndPointConstant.Teacher.DeleteTeacher)]
        [ProducesResponseType(typeof(Teacher), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundObjectResult))]
        public async Task<IActionResult> DeleteTeacher([FromRoute] Guid teacherId)
        {
            var teacher = await _teacherService.RemoveTeacher(teacherId);
            if (teacher == null)
            {
                return NotFound(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Error = "Teacher not found",
                    TimeStamp = TimeUtils.GetCurrentSEATime()
                });
            }

            return Ok(teacher);
        }


        [HttpPatch(ApiEndPointConstant.Teacher.UpdateTeacher)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
        public async Task<IActionResult> UpdateTeacherInformation(Guid id, [FromBody] UpdateTecherRequest updateTeacherRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = MessageConstant.TeacherMessage.InvalidTeacherData,
                    TimeStamp = TimeUtils.GetCurrentSEATime()
                });
            }

            try
            {
                var isSuccessful = await _teacherService.UpdateTeacher(id, updateTeacherRequest);
                if (!isSuccessful)
                {
                    return BadRequest(new ErrorResponse()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Error = MessageConstant.TeacherMessage.UpdateTeacherFailedMessage,
                        TimeStamp = TimeUtils.GetCurrentSEATime()
                    });
                }

                return Ok(MessageConstant.TeacherMessage.UpdateTeacherSuccessfulMessage);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message,
                    TimeStamp = TimeUtils.GetCurrentSEATime()
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Error = ex.Message,
                    TimeStamp = TimeUtils.GetCurrentSEATime()
                });
            }

        }
    }
}

