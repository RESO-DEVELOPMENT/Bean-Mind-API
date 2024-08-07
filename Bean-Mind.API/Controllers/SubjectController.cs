﻿using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Subjects;
using Bean_Mind.API.Payload.Response.Chapters;
using Bean_Mind.API.Payload.Response.Curriculums;
using Bean_Mind.API.Payload.Response.Subjects;
using Bean_Mind.API.Service.Implement;
using Bean_Mind.API.Service.Interface;
using Bean_Mind_Data.Paginate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bean_Mind.API.Controllers
{
    [ApiController]
    public class SubjectController : BaseController<SubjectController>
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ILogger<SubjectController> logger, ISubjectService subjectService) : base(logger)
        {
            _subjectService = subjectService;
        }

        [Authorize(Roles = "SysSchool")]
        [HttpPost(ApiEndPointConstant.Subject.Create)]
        [ProducesResponseType(typeof(CreateNewSubjectResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> CreateSubject([FromBody] CreateNewSubjectRequest createNewSubjectRequest, [FromQuery] Guid courseId)
        {
            CreateNewSubjectResponse response = await _subjectService.CreateNewSubject(createNewSubjectRequest, courseId);
            if (response == null)
            {
                return Problem(MessageConstant.SubjectMessage.CreateNewSubjectFailedMessage);
            }
            return CreatedAtAction(nameof(CreateSubject), response);
        }

        [HttpGet(ApiEndPointConstant.Subject.GetAll)]
        [ProducesResponseType(typeof(IPaginate<GetSubjectResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetListSubject([FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _subjectService.getListSubject(pageNumber, pageSize);
            if (response == null)
            {
                return Problem(MessageConstant.SubjectMessage.SubjectsIsEmpty);
            }
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Subject.GetById)]
        [ProducesResponseType(typeof(GetSubjectResponse), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetSubject([FromRoute] Guid id)
        {
            var response = await _subjectService.getSubjectById(id);

            return Ok(response);
        }

        [Authorize(Roles = "SysSchool")]
        [HttpDelete(ApiEndPointConstant.Subject.DeleteSubject)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteSubject([FromRoute] Guid id)
        {

            var response = await _subjectService.RemoveSubject(id);

            return Ok(response);

        }

        [Authorize(Roles = "SysSchool")]
        [HttpPatch(ApiEndPointConstant.Subject.UpdateSubject)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateSubject([FromBody] UpdateSubjectRequest request, [FromRoute] Guid id, [FromQuery] Guid courseId)
        {

            var response = await _subjectService.UpdateSubject(id, request, courseId);
            if (response == false)
            {
                return Problem(MessageConstant.SubjectMessage.UpdateSubjectFailedMessage);
            }

            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Subject.GetChapterInSubject)]
        [ProducesResponseType(typeof(IPaginate<GetChapterResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetChapterInSubject([FromRoute] Guid id, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _subjectService.GetListChapter(id, pageNumber, pageSize);

            return Ok(response);
        }
        [HttpGet(ApiEndPointConstant.Subject.GetSubjectByTitle)]
        [ProducesResponseType(typeof(IPaginate<GetSubjectResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetSubjectByTitle([FromQuery] string title, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _subjectService.GetListSubjectByTitle(title, pageNumber, pageSize);
            if (response == null)
            {
                return Problem(MessageConstant.SubjectMessage.SubjectsIsEmpty);
            }

            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Subject.GetSubjectByCode)]
        [ProducesResponseType(typeof(IPaginate<GetSubjectResponse>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        public async Task<IActionResult> GetCourseByCode([FromQuery] string code, [FromQuery] int? page, [FromQuery] int? size)
        {
            int pageNumber = page ?? 1;
            int pageSize = size ?? 10;
            var response = await _subjectService.GetListSubjectByCode(code, pageNumber, pageSize);
            if (response == null)
            {
                return Problem(MessageConstant.SubjectMessage.SubjectsIsEmpty);
            }

            return Ok(response);
        }
    }
}
