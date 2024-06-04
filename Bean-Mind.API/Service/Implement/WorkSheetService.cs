using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.WorkSheets;
using Bean_Mind.API.Payload.Response.WorkSheets;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using System.Drawing;

namespace Bean_Mind.API.Service.Implement
{
    public class WorkSheetService : BaseService<WorkSheetService>, IWorkSheetService
    {
        public WorkSheetService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<WorkSheetService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateNewWorkSheetResponse> CreateNewWorkSheet(CreateNewWorkSheetRequest request, Guid activityId, Guid worksheetTemplateId)
        {
            _logger.LogInformation($"Create new WorkSheet with {request.Title}");
            if (activityId == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.ActivityMessage.ActivityNotFound);
            }
            if (worksheetTemplateId == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.WorkSheetTemplateMessage.WorkSheetTemplateNotFound);
            }
            Activity activity = await _unitOfWork.GetRepository<Activity>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(activityId) && s.DelFlg != true);
            if (activity == null)
            {
                throw new BadHttpRequestException(MessageConstant.ActivityMessage.ActivityNotFound);
            }
            WorksheetTemplate template = await _unitOfWork.GetRepository<WorksheetTemplate>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(worksheetTemplateId) && s.DelFlg != true);
            if (template == null)
            {
                throw new BadHttpRequestException(MessageConstant.WorkSheetTemplateMessage.WorkSheetTemplateNotFound);
            }
            WorkSheet workSheet = new WorkSheet
            {
                Id = new Guid(),
                Title = request.Title,
                Description = request.Description,
                ActivityId = activityId,
                WorksheetTemplateId = worksheetTemplateId,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false
            };
            await _unitOfWork.GetRepository<WorkSheet>().InsertAsync(workSheet);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            CreateNewWorkSheetResponse response = null;
            if(isSuccess)
            {
                response = new CreateNewWorkSheetResponse()
                {
                    Id = workSheet.Id,
                    Title = workSheet.Title,
                    Description = workSheet.Description,
                    ActivityId = workSheet.ActivityId,
                    WorksheetTemplateId = workSheet.WorksheetTemplateId,
                    InsDate = workSheet.InsDate,
                    UpdDate = workSheet.UpdDate,
                    DelFlg = workSheet.DelFlg
                };
            }
            return response;
        }

        public async Task<bool> DeleteWorkSheet(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.WorkSheetMessage.WorkSheetNotFound);
            }
            var workSheet = await _unitOfWork.GetRepository<WorkSheet>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (workSheet == null)
            {
                throw new BadHttpRequestException(MessageConstant.WorkSheetMessage.WorkSheetNotFound);
            }
            workSheet.DelFlg = true;
            workSheet.UpdDate = TimeUtils.GetCurrentSEATime();

            _unitOfWork.GetRepository<WorkSheet>().UpdateAsync(workSheet);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }

        public async Task<IPaginate<GetWorkSheetResponse>> GetWorkSheet(int page, int size)
        {
            var workSheets = await _unitOfWork.GetRepository<WorkSheet>().GetPagingListAsync(
                selector: w => new GetWorkSheetResponse
                {
                    Id = w.Id,
                    Title = w.Title,
                    Description = w.Description,
                },
                predicate: w => w.DelFlg == false,
                page: page,
                size: size);
            return workSheets;
        }

        public async Task<GetWorkSheetResponse> GetWorkSheetById(Guid id)
        {
            var workSheet = await _unitOfWork.GetRepository<WorkSheet>().SingleOrDefaultAsync(
                selector: w => new GetWorkSheetResponse
                {
                    Id = w.Id,
                    Title = w.Title,
                    Description = w.Description,
                },
                predicate: w => w.Id.Equals(id) && w.DelFlg == false);
            return workSheet;
        }

        public async Task<bool> UpdateWorkSheet(Guid id, UpdateWorkSheetRequest request, Guid activityId, Guid worksheetTemplateId)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.WorkSheetMessage.WorkSheetNotFound);
            }
            var worksheet = await _unitOfWork.GetRepository<WorkSheet>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (worksheet == null)
            {
                throw new BadHttpRequestException(MessageConstant.WorkSheetMessage.WorkSheetNotFound);
            }
            if (activityId != Guid.Empty)
            {
                var activity = await _unitOfWork.GetRepository<Activity>().SingleOrDefaultAsync(predicate: a => a.Id.Equals(activityId) && a.DelFlg != true);
                if (activity == null)
                {
                    throw new BadHttpRequestException(MessageConstant.ActivityMessage.ActivityNotFound);
                }
                worksheet.ActivityId = activityId;
            }
            if (worksheetTemplateId != Guid.Empty)
            {
                var activity = await _unitOfWork.GetRepository<WorksheetTemplate>().SingleOrDefaultAsync(predicate: w => w.Id.Equals(worksheetTemplateId) && w.DelFlg != true);
                if (activity == null)
                {
                    throw new BadHttpRequestException(MessageConstant.WorkSheetTemplateMessage.WorkSheetTemplateNotFound);
                }
                worksheet.WorksheetTemplateId = worksheetTemplateId;
            }
            worksheet.Title = string.IsNullOrEmpty(request.Title) ? worksheet.Title : request.Title;
            worksheet.Description = string.IsNullOrEmpty(request.Description) ? worksheet.Description : request.Description;
            worksheet.UpdDate = TimeUtils.GetCurrentSEATime();
            _unitOfWork.GetRepository<WorkSheet>().UpdateAsync(worksheet);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }
    }
}
