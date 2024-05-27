﻿using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload;
using Bean_Mind.API.Payload.Request.Schools;
using Bean_Mind.API.Payload.Response.Schools;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
{
    [ApiController]
    public class SchoolController : BaseController<SchoolController>
    {
        private readonly ISchoolService _schoolService;
        public SchoolController(ILogger<SchoolController> logger, ISchoolService schoolService) : base(logger)
        {
            _schoolService = schoolService;
        }

        [HttpPost(ApiEndPointConstant.School.CreateSchool)]
        [ProducesResponseType(typeof(CreateNewSchoolResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateSchool([FromBody] CreateNewSchoolRequest createNewSchoolRequest)
        {

            CreateNewSchoolResponse response =
                await _schoolService.CreateNewSchool(createNewSchoolRequest);
            if (response == null)
            {
                return Problem(MessageConstant.SchoolMessage.CreateNewSchoolFailedMessage);
            }

            return CreatedAtAction(nameof(CreateSchool), response);
        }

        [HttpGet(ApiEndPointConstant.School.GetListSchool)]
        [ProducesResponseType(typeof(IPaginate<GetSchoolResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListSchool([FromQuery] int page,
            [FromQuery] int size)
        {

            var response = await _schoolService.getListSchool(page, size);
            if (response == null)
            {
                return Problem(MessageConstant.SchoolMessage.CreateNewSchoolFailedMessage);
            }

            return Ok(response);

        }

        [HttpGet(ApiEndPointConstant.School.GetSchool)]
        [ProducesResponseType(typeof(GetSchoolResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetSchool([FromRoute] Guid id)
        {

            var response = await _schoolService.getSchoolById(id);

            return Ok(response);

        }

        [HttpDelete(ApiEndPointConstant.School.DeleteSchool)]
        [ProducesResponseType(typeof(GetSchoolResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteSchool([FromRoute] Guid id)
        {

            var response = await _schoolService.deleteSchool(id);

            return Ok(response);

        }

        [HttpPatch(ApiEndPointConstant.School.UpdateSchool)]
        [ProducesResponseType(typeof(GetSchoolResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateSchool([FromBody] CreateNewSchoolRequest createNewSchoolRequest, [FromRoute] Guid id)
        {

            var response = await _schoolService.updateSchool(createNewSchoolRequest, id);
            if (response == false)
            {
                return Problem(MessageConstant.SchoolMessage.UpdateSchoolFailedMessage);
            }

            return Ok(response);
        }
    }
}
