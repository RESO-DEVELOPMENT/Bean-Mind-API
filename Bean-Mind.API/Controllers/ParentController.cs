﻿using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Parents;
using Bean_Mind.API.Payload.Response.Parents;
using Bean_Mind.API.Payload;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Mvc;
using Bean_Mind.API.Service.Implement;
using Bean_Mind_Data.Models;

namespace Bean_Mind.API.Controllers
{
    [Route(ApiEndPointConstant.Parent.ParentEndpoint)]
    [ApiController]
    public class ParentController : BaseController<ParentController>
    {
        private readonly IParentService _parentService;

        public ParentController(ILogger<ParentController> logger, IParentService parentService) : base(logger)
        {
            _parentService = parentService;
        }

        [HttpPost(ApiEndPointConstant.Parent.Create)]
        [ProducesResponseType(typeof(CreateNewParentResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
        public async Task<IActionResult> CreateParent([FromBody] CreateNewParentResquest newParentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = "Invalid input data",
                    TimeStamp = DateTime.Now
                });
            }

            var response = await _parentService.AddParent(newParentRequest);
            if (response == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = "Failed to create parent",
                    TimeStamp = DateTime.Now
                });
            }

            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Parent.GetAll)]
        [ProducesResponseType(typeof(IPaginate<ParentResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllParents([FromQuery] int page, [FromQuery] int size)
        {
            var parents = await _parentService.GetAllParents(page, size);
            return Ok(parents);
        }

        [HttpGet(ApiEndPointConstant.Parent.GetById)]
        [ProducesResponseType(typeof(ParentResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundObjectResult))]
        public async Task<IActionResult> GetParentById([FromRoute] Guid parentId)
        {
            var parent = await _parentService.GetParentById(parentId);
            if (parent == null)
            {
                return NotFound(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Error = "Parent not found",
                    TimeStamp = DateTime.Now
                });
            }

            return Ok(parent);
        }

        [HttpPatch(ApiEndPointConstant.Parent.UpdateParent)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
        public async Task<IActionResult> UpdateParentInformation(Guid id, [FromBody] UpdateParentRequest updateParentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = "Invalid parent data",
                    TimeStamp = DateTime.Now
                });
            }

            try
            {
                var isSuccessful = await _parentService.UpdateParent(id, updateParentRequest);
                if (!isSuccessful)
                {
                    return BadRequest(new ErrorResponse()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Error = "Failed to update parent",
                        TimeStamp = DateTime.Now
                    });
                }

                return Ok("Parent information updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Error = ex.Message,
                    TimeStamp = DateTime.Now
                });
            }
        }

        [HttpDelete(ApiEndPointConstant.Parent.DeleteParent)]
        [ProducesResponseType(typeof(Parent), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(NotFoundObjectResult))]
        public async Task<IActionResult> RemoveParents(Guid Id)
        {

            var response = await _parentService.RemoveParent(Id);

            return Ok(response);

        }
    }
}