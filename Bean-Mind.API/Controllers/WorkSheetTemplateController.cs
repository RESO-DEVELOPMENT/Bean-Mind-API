using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.WorkSheetTemplates;
using Bean_Mind.API.Payload.Response.WorkSheetTemplates;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
{
    [ApiController]
    public class WorkSheetTemplateController : BaseController<WorkSheetTemplateController>
    {
        private readonly IWorkSheetTemplateService _workSheetTemplateService;
        public WorkSheetTemplateController(ILogger<WorkSheetTemplateController> logger, IWorkSheetTemplateService workSheetTemplateService) : base(logger)
        {
            _workSheetTemplateService = workSheetTemplateService;
        }

        [Authorize(Roles = "Teacher,SysSchool")]
        [HttpPost(ApiEndPointConstant.WorkSheetTemplate.Create)]
        [ProducesResponseType(typeof(CreateNewWorkSheetTemplateResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateWorkSheetTemplate([FromBody] CreateNewWorkSheetTemplateRequest createNewWorkSheetTemplateRequest, [FromQuery] Guid subjectId)
        {
            CreateNewWorkSheetTemplateResponse response = await _workSheetTemplateService.CreateNewWorkSheetTemplate(createNewWorkSheetTemplateRequest, subjectId);
            if (response == null)
            {
                return Problem(MessageConstant.WorkSheetTemplateMessage.CreateNewWorkSheetTemplateFailedMessage);
            }
            return CreatedAtAction(nameof(CreateWorkSheetTemplate), response);
        }

        [HttpGet(ApiEndPointConstant.WorkSheetTemplate.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetWorkSheetTemplateResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListWorkSheetTemplate([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _workSheetTemplateService.GetListWorkSheetTemplate(pageNumber, pageSize);
            if (response == null)
            {
                return Problem(MessageConstant.WorkSheetTemplateMessage.WorkSheetTemplateIsEmpty);
            }
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.WorkSheetTemplate.GetById)]
        [ProducesResponseType(typeof(GetWorkSheetTemplateResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetWorkSheetTemplate([FromRoute] Guid id)
        {
            var response = await _workSheetTemplateService.GetWorkSheetTemplateById(id);

            return Ok(response);
        }

        [Authorize(Roles = "Teacher,SysSchool")]
        [HttpDelete(ApiEndPointConstant.WorkSheetTemplate.DeleteWorkSheetTemplate)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteWorkSheetTemplate([FromRoute] Guid id)
        {
            var response = await _workSheetTemplateService.RemoveWorkSheetTemplate(id);

            return Ok(response);
        }

        [Authorize(Roles = "Teacher,SysSchool")]
        [HttpPatch(ApiEndPointConstant.WorkSheetTemplate.UpdateWorkSheetTemplate)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateWorkSheetTemplate([FromBody] UpdateWorkSheetTemplateRequest request, [FromRoute] Guid id, [FromQuery] Guid subjectId)
        {

            var response = await _workSheetTemplateService.UpdateWorkSheetTemplate(id, request, subjectId);
            if (response == false)
            {
                return Problem(MessageConstant.WorkSheetTemplateMessage.UpdateWorkSheetTemplateFailedMessage);
            }

            return Ok(response);
        }
    }
}
