using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Documents;
using Bean_Mind.API.Payload.Response.Documents;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
{
    public class DocumentController :  BaseController<DocumentController>
    {
        private readonly IDocumentService _documentService;
        public DocumentController(ILogger<DocumentController> logger, IDocumentService documentService) : base(logger)
        {
            _documentService = documentService;
        }
        [HttpPost(ApiEndPointConstant.Document.Create)]
        [ProducesResponseType(typeof(CreateNewDocumentResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateDocument([FromForm] CreateNewDocumentRequest createNewDocumentRequest, [FromQuery] Guid activityId)
        {
            CreateNewDocumentResponse response = await _documentService.CreateNewDocument(createNewDocumentRequest, activityId);
            if (response == null)
            {
                return Problem(MessageConstant.ChapterMessage.CreateNewChapterFailedMessage);
            }

            return CreatedAtAction(nameof(CreateDocument), response);
        }

        [HttpGet(ApiEndPointConstant.Document.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetDocumentResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListDocument([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _documentService.GetListDocument(pageNumber, pageSize);
            if (response == null)
            {
                return Problem(MessageConstant.DocumentMessage.DocumentIdEmpty);
            }
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Document.GetById)]
        [ProducesResponseType(typeof(GetDocumentResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetDocument([FromRoute] Guid id)
        {
            var response = await _documentService.GetDocumentById(id);

            return Ok(response);
        }

        [HttpDelete(ApiEndPointConstant.Document.DeleteDocument)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteDocument([FromRoute] Guid id)
        {

            var response = await _documentService.RemoveDocument(id);

            return Ok(response);

        }

        [HttpPatch(ApiEndPointConstant.Document.UpdateDocument)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateDocument([FromBody] UpdateDocumentRequest request, [FromRoute] Guid id, [FromQuery] Guid activityId)
        {

            var response = await _documentService.UpdateDocument(id, request, activityId);
            if (response == false)
            {
                return Problem(MessageConstant.DocumentMessage.UpdateDocumentFailedMessage);
            }

            return Ok(response);
        }

    }
}
