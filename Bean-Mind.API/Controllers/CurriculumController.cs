using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Curriculums;
using Bean_Mind.API.Payload.Response.Courses;
using Bean_Mind.API.Payload.Response.Curriculums;
using Bean_Mind.API.Service.Implement;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
{
    public class CurriculumController : BaseController<CurriculumController>
    {
        private readonly ICurriculumService _curriculumService;
        public CurriculumController(ILogger<CurriculumController> logger, ICurriculumService curriculumService) : base(logger)
        {
            _curriculumService = curriculumService;
        }

        [Authorize(Roles = "SysSchool")]
        [HttpPost(ApiEndPointConstant.Curriculum.Create)]
        [ProducesResponseType(typeof(CreateNewCurriculumResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateCurriculum([FromBody] CreateNewCurriculumRequest createNewCurriculumRequest)
        {
            CreateNewCurriculumResponse response = await _curriculumService.CreateNewCurriculum(createNewCurriculumRequest);
            if (response == null)
            {
                return Problem(MessageConstant.CurriculumMessage.CreateNewCurriculumFailedMessage);
            }

            return CreatedAtAction(nameof(CreateCurriculum), response);
        }

        [HttpGet(ApiEndPointConstant.Curriculum.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetCurriculumResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListCurriculum([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _curriculumService.getListCurriculum(pageNumber, pageSize);
            if (response == null)
            {
                return Problem(MessageConstant.CurriculumMessage.CurriculumsIsEmpty);
            }

            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Curriculum.GetById)]
        [ProducesResponseType(typeof(GetCurriculumResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetCurriculum([FromRoute] Guid id)
        {
            var response = await _curriculumService.getCurriculumById(id);

            return Ok(response);
        }

        [Authorize(Roles = "SysSchool")]
        [HttpDelete(ApiEndPointConstant.Curriculum.DeleteCurriculum)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteCurriculum([FromRoute] Guid id)
        {
            var response = await _curriculumService.deleteCurriculum(id);
            return Ok(response);
        }

        [Authorize(Roles = "SysSchool")]
        [HttpPatch(ApiEndPointConstant.Curriculum.GetById)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateCurriculum([FromRoute] Guid id, [FromBody] UpdateCurriculumRequest request, [FromQuery] Guid schoolId)
        {
            var response = await _curriculumService.updateCurriculum(id, request, schoolId);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Curriculum.GetCourseInCurriculum)]
        [ProducesResponseType(typeof(IPaginate<GetCourseResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetCourseInCurriculum([FromRoute] Guid id, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _curriculumService.GetListCourses(id, pageNumber, pageSize);

            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Curriculum.GetCurriculumByTitle)]
        [ProducesResponseType(typeof(IPaginate<GetCurriculumResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetCurriculumByTitle([FromQuery] string title, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _curriculumService.GetListCurriculumByTitle(title, pageNumber, pageSize);
            if (response == null)
            {
                return Problem(MessageConstant.CurriculumMessage.CurriculumsIsEmpty);
            }

            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Curriculum.GetCurriculumByCode)]
        [ProducesResponseType(typeof(IPaginate<GetCurriculumResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetCurriculumByCode([FromQuery] string code, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _curriculumService.GetListCurriculumByCode(code, pageNumber, pageSize);
            if (response == null)
            {
                return Problem(MessageConstant.CurriculumMessage.CurriculumsIsEmpty);
            }

            return Ok(response);
        }

    }
}
