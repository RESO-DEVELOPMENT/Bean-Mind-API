using Bean_Mind.API.Constants;
using Microsoft.AspNetCore.Mvc;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Payload.Request.TeacherTeachables;
using Bean_Mind.API.Payload.Response.TeacherTeachables;
using Bean_Mind_Data.Paginate;
using Bean_Mind.API.Payload;
using Bean_Mind.API.Utils;


namespace Bean_Mind.API.Controllers
{
    [Route(ApiEndPointConstant.TeacherTeachable.TeacherTeachableEndpoint)]
    [ApiController]
    public class TeacherTeachableController : BaseController<TeacherTeachableController>
    {
        private readonly ITeacherTeachableService _teacherTeachableService;

        public TeacherTeachableController(ILogger<TeacherTeachableController> logger, ITeacherTeachableService teacherTeachableService) : base(logger)
        {
            _teacherTeachableService = teacherTeachableService;
        }

        [HttpPost(ApiEndPointConstant.TeacherTeachable.Create + "/{TeacherId}/{SubjectId}")]
        [ProducesResponseType(typeof(GetTeacherTeachableResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
        public async Task<IActionResult> CreateTeacherTeachable([FromRoute] Guid TeacherId,[FromRoute] Guid SubjectId)
        {
            var request = new CreateNewTeacherTeachableRequest
            {
                TeacherId = TeacherId,
                SubjectId = SubjectId
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = MessageConstant.TeacherTeachableMessage.InvalidInputData,
                    TimeStamp = TimeUtils.GetCurrentSEATime()
                });
            }

            var response = await _teacherTeachableService.CreateTeacherTeachable(request);
            if (response == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = MessageConstant.TeacherTeachableMessage.CreateFailed,
                    TimeStamp = TimeUtils.GetCurrentSEATime()
                });
            }

            return Ok(response);
        }


        [HttpGet(ApiEndPointConstant.TeacherTeachable.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetTeacherTeachableResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTeacherTeachables([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var teacherTeachables = await _teacherTeachableService.GetAllTeacherTeachables(pageNumber, pageSize);
            return Ok(teacherTeachables);
        }

        [HttpGet(ApiEndPointConstant.TeacherTeachable.GetTeacherTeachablesByTeacher)]
        [ProducesResponseType(typeof(IEnumerable<GetTeacherTeachableResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundObjectResult))]
        public async Task<IActionResult> GetTeacherTeachablesByTeacher([FromRoute] Guid teacherId)
        {
            try
            {
                var teacherTeachables = await _teacherTeachableService.GetTeacherTeachablesByTeacher(teacherId);
                return Ok(teacherTeachables);
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

        [HttpGet(ApiEndPointConstant.TeacherTeachable.GetTeacherTeachablesBySubject)]
        [ProducesResponseType(typeof(IEnumerable<GetTeacherTeachableResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundObjectResult))]
        public async Task<IActionResult> GetTeacherTeachablesBySubject([FromRoute] Guid subjectId)
        {
            try
            {
                var teacherTeachables = await _teacherTeachableService.GetTeacherTeachablesBySubject(subjectId);
                return Ok(teacherTeachables);
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

        [HttpDelete(ApiEndPointConstant.TeacherTeachable.DeleteTeacherTeachable)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundObjectResult))]
        public async Task<IActionResult> DeleteTeacherTeachable([FromRoute] Guid id)
        {
            var isDeleted = await _teacherTeachableService.RemoveTeacherTeachable(id);
            if (!isDeleted)
            {
                return NotFound(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Error = MessageConstant.TeacherTeachableMessage.NotFound,
                    TimeStamp = TimeUtils.GetCurrentSEATime()
                });
            }

            return Ok(isDeleted);
        }

        [HttpPatch(ApiEndPointConstant.TeacherTeachable.UpdateTeacherTeachable)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
        public async Task<IActionResult> UpdateTeacherTeachable([FromRoute] Guid teacherId, [FromRoute] Guid subjectId, [FromBody] UpdateTeacherTeachableRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = MessageConstant.TeacherTeachableMessage.InvalidData,
                    TimeStamp = TimeUtils.GetCurrentSEATime()
                });
            }

            try
            {
                var isSuccessful = await _teacherTeachableService.UpdateTeacherTeachable(teacherId, subjectId, request);
                if (!isSuccessful)
                {
                    return BadRequest(new ErrorResponse()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Error = MessageConstant.TeacherTeachableMessage.UpdateFailed,
                        TimeStamp = TimeUtils.GetCurrentSEATime()
                    });
                }

                return Ok(MessageConstant.TeacherTeachableMessage.UpdateSuccessful);
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
