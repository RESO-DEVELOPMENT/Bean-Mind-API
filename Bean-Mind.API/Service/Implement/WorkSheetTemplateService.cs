using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.WorkSheetTemplates;
using Bean_Mind.API.Payload.Response.Subjects;
using Bean_Mind.API.Payload.Response.WorkSheetTemplates;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind.API.Service.Implement
{
    public class WorkSheetTemplateService : BaseService<WorkSheetTemplateService>, IWorkSheetTemplateService
    {
        public WorkSheetTemplateService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<WorkSheetTemplateService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateNewWorkSheetTemplateResponse> CreateNewWorkSheetTemplate(CreateNewWorkSheetTemplateRequest request, Guid subjectId)
        {
            _logger.LogInformation($"Create new WorkSheet Template with {request.Title}");
            if (subjectId == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
            }
            var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(subjectId) && s.DelFlg != true);
            if (subject == null)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
            }

            WorksheetTemplate worksheetTemplate = new WorksheetTemplate()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                EasyCount = request.EasyCount,
                MediumCount = request.MediumCount,
                HardCount = request.HardCount,
                SubjectId = subjectId,
                DelFlg = false,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime()
            };
            await _unitOfWork.GetRepository<WorksheetTemplate>().InsertAsync(worksheetTemplate);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            CreateNewWorkSheetTemplateResponse create = null;
            if (isSuccessful)
            {
                create = new CreateNewWorkSheetTemplateResponse()
                {
                    Id = worksheetTemplate.Id,
                    Title = worksheetTemplate.Title,
                    EasyCount = worksheetTemplate.EasyCount,
                    MediumCount = worksheetTemplate.MediumCount,
                    HardCount = worksheetTemplate.HardCount,
                    SubjectId = worksheetTemplate.SubjectId,
                    DelFlg = worksheetTemplate.DelFlg,
                    InsDate = worksheetTemplate.InsDate,
                    UpdDate= worksheetTemplate.UpdDate
                };
            }
            return create;
        }
        public async Task<IPaginate<GetWorkSheetTemplateResponse>> GetListWorkSheetTemplate(int page, int size)
        {
            var worksheetTemplates = await _unitOfWork.GetRepository<WorksheetTemplate>().GetPagingListAsync(
                selector: s => new GetWorkSheetTemplateResponse(s.Id, s.Title, s.EasyCount, s.MediumCount, s.HardCount, s.SubjectId),
                predicate: s => s.DelFlg != true,
                page: page,
                size: size
                );
            if (worksheetTemplates == null)
            {
                throw new BadHttpRequestException(MessageConstant.WorkSheetTemplateMessage.WorkSheetTemplateIsEmpty);
            }
            return worksheetTemplates;
        }
        public async Task<GetWorkSheetTemplateResponse> GetWorkSheetTemplateById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.WorkSheetTemplateMessage.WorkSheetTemplateNotFound);
            }
            var worksheetTemplate = await _unitOfWork.GetRepository<WorksheetTemplate>().SingleOrDefaultAsync(
                selector: s => new GetWorkSheetTemplateResponse(s.Id, s.Title, s.EasyCount, s.MediumCount, s.HardCount, s.SubjectId),
                predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (worksheetTemplate == null)
            {
                throw new BadHttpRequestException(MessageConstant.WorkSheetTemplateMessage.WorkSheetTemplateNotFound);
            }
            return worksheetTemplate;
        }
        public async Task<bool> UpdateWorkSheetTemplate(Guid id, UpdateWorkSheetTemplateRequest request, Guid subjectId)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.WorkSheetMessage.WorkSheetNotFound);
            }
            var worksheetTemplate = await _unitOfWork.GetRepository<WorksheetTemplate>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (worksheetTemplate == null)
            {
                throw new BadHttpRequestException(MessageConstant.WorkSheetMessage.WorkSheetNotFound);
            }

            if (subjectId != Guid.Empty)
            {
                var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(predicate: c => c.Id.Equals(subjectId) && c.DelFlg != true);
                if (subject == null)
                {
                    throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
                }
                worksheetTemplate.SubjectId = subjectId;
            }

            worksheetTemplate.Title = string.IsNullOrEmpty(request.Title) ? worksheetTemplate.Title : request.Title;
            worksheetTemplate.EasyCount = request.EasyCount ?? worksheetTemplate.EasyCount;
            worksheetTemplate.MediumCount = request.MediumCount ?? worksheetTemplate.MediumCount;
            worksheetTemplate.HardCount = request.HardCount ?? worksheetTemplate.HardCount;

            _unitOfWork.GetRepository<WorksheetTemplate>().UpdateAsync(worksheetTemplate);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> RemoveWorkSheetTemplate(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.WorkSheetTemplateMessage.WorkSheetTemplateNotFound);
            }
            var worksheetTemplate = await _unitOfWork.GetRepository<WorksheetTemplate>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (worksheetTemplate == null)
            {
                throw new BadHttpRequestException(MessageConstant.WorkSheetTemplateMessage.WorkSheetTemplateNotFound);
            }

            worksheetTemplate.UpdDate = TimeUtils.GetCurrentSEATime();
            worksheetTemplate.DelFlg = true;

            var worksheets = await _unitOfWork.GetRepository<WorkSheet>().GetListAsync(
                predicate: s => s.WorksheetTemplateId.Equals(id) && s.DelFlg != true,
                include: s => s.Include(s => s.WorksheetQuestions)
                );

            foreach ( var worksheet in worksheets)
            {
                var worksheetQuestions = worksheet.WorksheetQuestions.Where(s => s.DelFlg != true).ToList();
                foreach (var worksheetQuestion in worksheetQuestions)
                {
                    worksheetQuestion.UpdDate = TimeUtils.GetCurrentSEATime();
                    worksheetQuestion.DelFlg = true;
                    _unitOfWork.GetRepository<WorksheetQuestion>().UpdateAsync(worksheetQuestion);
                }
                worksheet.UpdDate = TimeUtils.GetCurrentSEATime();
                worksheet.DelFlg = true;
                _unitOfWork.GetRepository<WorkSheet>().UpdateAsync(worksheet);
            }

            _unitOfWork.GetRepository<WorksheetTemplate>().UpdateAsync(worksheetTemplate);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
