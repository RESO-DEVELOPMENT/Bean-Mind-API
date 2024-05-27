
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Curriculums;
using Bean_Mind.API.Payload.Response.Curriculums;
using Bean_Mind.API.Service.Implement;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
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
        public async Task<IActionResult> GetListCurriculum([FromQuery] int page, [FromQuery] int size)
        {
            var response = await _curriculumService.getListCurriculum(page, size);
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

        [HttpDelete(ApiEndPointConstant.Curriculum.DeleteCurriculum)]
        [ProducesResponseType(typeof(GetCurriculumResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteCurriculum([FromRoute] Guid id)
        {
            var response = await _curriculumService.deleteCurriculum(id);
            return Ok(response);
        }

        [HttpPatch(ApiEndPointConstant.Curriculum.GetById)]
        [ProducesResponseType(typeof(GetCurriculumResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateCurriculum([FromRoute] Guid id, [FromBody] UpdateCurriculumRequest request, [FromQuery] Guid schoolId)
        {
            var response = await _curriculumService.updateCurriculum(id, request, schoolId);
            return Ok(response);
        }


    }
}
