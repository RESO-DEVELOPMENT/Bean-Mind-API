using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.WorkSheets;
using Bean_Mind.API.Payload.Response.WorkSheets;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
{
    public class WorkSheetController : BaseController<WorkSheetController>
    {
        private readonly IWorkSheetService _workSheetService;
        public WorkSheetController(ILogger<WorkSheetController> logger, IWorkSheetService workSheetService) : base(logger)
        {
            _workSheetService = workSheetService;
        }

        [HttpPost(ApiEndPointConstant.WorkSheet.Create)]
        [ProducesResponseType(typeof(CreateNewWorkSheetResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateNewWorkSheet([FromBody] CreateNewWorkSheetRequest request, [FromQuery] Guid activityId, [FromQuery] Guid workSheetTemplateId)
        {
            CreateNewWorkSheetResponse response = await _workSheetService.CreateNewWorkSheet(request, activityId, workSheetTemplateId);
            if (response == null)
            {
                return Problem(MessageConstant.WorkSheetMessage.CreateNewWorkSheetFailedMessage);
            }
            return CreatedAtAction(nameof(CreateNewWorkSheet), response);
        }

        [HttpGet(ApiEndPointConstant.WorkSheet.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetWorkSheetResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListWorkSheet([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _workSheetService.GetWorkSheet(pageNumber, pageSize);
            if (response == null)
            {
                return Problem(MessageConstant.WorkSheetMessage.WorkSheetIsEmpty);
            }
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.WorkSheet.GetById)]
        [ProducesResponseType(typeof(GetWorkSheetResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetWorkSheetById([FromRoute] Guid id)
        {
            var response = await _workSheetService.GetWorkSheetById(id);
            return Ok(response);
        }

        [HttpDelete(ApiEndPointConstant.WorkSheet.DeleteWorkSheet)]
        [ProducesResponseType(typeof(Boolean), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteWorkSheet([FromRoute] Guid id)
        {
            var response = await _workSheetService.DeleteWorkSheet(id);
            return Ok(response);
        }

        [HttpPatch(ApiEndPointConstant.WorkSheet.UpdateWorkSheet)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateWorkSheet([FromRoute] Guid id,[FromBody] UpdateWorkSheetRequest request, [FromQuery] Guid activityId, [FromQuery] Guid worksheetTemplateId)
        {

            var response = await _workSheetService.UpdateWorkSheet(id, request, activityId, worksheetTemplateId);
            if (response == false)
            {
                return Problem(MessageConstant.WorkSheetMessage.UpdateWorkSheetFailedMessage);
            }

            return Ok(response);
        }

    }
}
