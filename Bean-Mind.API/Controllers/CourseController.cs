using Bean_Mind.API.Constants;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Mvc;
using Bean_Mind.API.Payload.Request.Courses;
using Bean_Mind.API.Payload.Response.Courses;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
    {
        public class CourseController :BaseController<CourseController>
        {
            private readonly ICourseService _courseService;
            public CourseController(ILogger<CourseController> logger, ICourseService courseService) : base(logger)
            {
            _courseService = courseService;
            }
            [HttpPost(ApiEndPointConstant.Course.Create)]
            [ProducesResponseType(typeof(CreateNewCourseResponse), StatusCodes.Status200OK)]
            [ProducesErrorResponseType(typeof(ProblemDetails))]
            public async Task<IActionResult> CreateCourse([FromBody] CreateNewCourseRequest createNewCourseRequest)
            {
                CreateNewCourseResponse response = await _courseService.CreateNewCourse(createNewCourseRequest);
                if (response == null)
                {
                    return Problem(MessageConstant.CourseMessage.CreateNewCourseFailedMessage);
                }

                return CreatedAtAction(nameof(CreateCourse), response);
            }

            [HttpGet(ApiEndPointConstant.Course.GetAll)]
            [ProducesResponseType(typeof(IPaginate<GetCourseResponse>), StatusCodes.Status200OK)]
            [ProducesErrorResponseType(typeof(ProblemDetails))]
            public async Task<IActionResult> GetListCourse([FromQuery] int page, [FromQuery] int size)
            {
                var response = await _courseService.GetListCourse(page, size);
                if (response == null)
                {
                    return Problem(MessageConstant.CourseMessage.CoursesIsEmpty);
                }

                return Ok(response);
            }

            [HttpGet(ApiEndPointConstant.Course.GetById)]
            [ProducesResponseType(typeof(GetCourseResponse), StatusCodes.Status200OK)]
            [ProducesErrorResponseType(typeof(ProblemDetails))]
            public async Task<IActionResult> GetCourse([FromRoute] Guid id)
            {
                var response = await _courseService.GetCourseById(id);

                return Ok(response);
            }

            [HttpDelete(ApiEndPointConstant.Course.DeleteCourse)]
            [ProducesResponseType(typeof(GetCourseResponse), StatusCodes.Status200OK)]
            [ProducesErrorResponseType(typeof(ProblemDetails))]
            public async Task<IActionResult> DeleteCurriculum([FromRoute] Guid id)
            {
                var response = await _courseService.DeleteCourse(id);
                return Ok(response);
            }

            [HttpPatch(ApiEndPointConstant.Course.GetById)]
            [ProducesResponseType(typeof(GetCourseResponse), StatusCodes.Status200OK)]
            [ProducesErrorResponseType(typeof(ProblemDetails))]
            public async Task<IActionResult> UpdateCourse([FromRoute] Guid id, [FromBody] UpdateCourseRequest request, [FromQuery] Guid curriculumId)
            {
                var response = await _courseService.UpdateCourse(id, request, curriculumId);
                return Ok(response);
            }


        }
    }


