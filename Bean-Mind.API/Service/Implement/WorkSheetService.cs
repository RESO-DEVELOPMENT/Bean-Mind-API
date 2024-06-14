using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.WorkSheets;
using Bean_Mind.API.Payload.Response.WorkSheets;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind.API.Service.Implement
{
    public class WorkSheetService : BaseService<WorkSheetService>, IWorkSheetService
    {
        public WorkSheetService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<WorkSheetService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateNewWorkSheetResponse> CreateNewWorkSheet(CreateNewWorkSheetRequest request, Guid activityId, Guid worksheetTemplateId)
        {
            _logger.LogInformation($"Creating new WorkSheet with title: {request.Title}");

            // Kiểm tra worksheetTemplateId trước, vì nó bắt buộc phải có
            if (worksheetTemplateId == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.WorkSheetTemplateMessage.WorkSheetTemplateNotFound);
            }

            var newWorkSheet = new WorkSheet
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false
            };

            if (activityId != Guid.Empty)
            {
                var activity = await _unitOfWork.GetRepository<Activity>().SingleOrDefaultAsync(
                    predicate: s => s.Id.Equals(activityId) && s.DelFlg != true);

                if (activity == null)
                {
                    throw new BadHttpRequestException(MessageConstant.ActivityMessage.ActivityNotFound);
                }

                newWorkSheet.ActivityId = activityId;
            }

            var template = await _unitOfWork.GetRepository<WorksheetTemplate>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(worksheetTemplateId) && s.DelFlg != true);

            if (template == null)
            {
                throw new BadHttpRequestException(MessageConstant.WorkSheetTemplateMessage.WorkSheetTemplateNotFound);
            }

            newWorkSheet.WorksheetTemplateId = worksheetTemplateId;

            await _unitOfWork.GetRepository<WorkSheet>().InsertAsync(newWorkSheet);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;

            CreateNewWorkSheetResponse response = null;
            if (isSuccess)
            {
                response = new CreateNewWorkSheetResponse()
                {
                    Id = newWorkSheet.Id,
                    Title = newWorkSheet.Title,
                    Description = newWorkSheet.Description,
                    ActivityId = newWorkSheet.ActivityId,
                    WorksheetTemplateId = newWorkSheet.WorksheetTemplateId,
                    InsDate = newWorkSheet.InsDate,
                    UpdDate = newWorkSheet.UpdDate,
                    DelFlg = newWorkSheet.DelFlg
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
            var workSheet = await _unitOfWork.GetRepository<WorkSheet>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(id) && s.DelFlg != true,
                include: s => s.Include(s => s.WorksheetQuestions)
                );
            if (workSheet == null)
            {
                throw new BadHttpRequestException(MessageConstant.WorkSheetMessage.WorkSheetNotFound);
            }
            workSheet.DelFlg = true;
            workSheet.UpdDate = TimeUtils.GetCurrentSEATime();

            var worksheetQuestions = workSheet.WorksheetQuestions.Where(x => x.DelFlg != true).ToList();
            foreach (var worksheetQuestion in worksheetQuestions)
            {
                worksheetQuestion.UpdDate = TimeUtils.GetCurrentSEATime();
                worksheetQuestion.DelFlg = true;
                _unitOfWork.GetRepository<WorksheetQuestion>().UpdateAsync(worksheetQuestion);
            }

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
                    ActivityId = w.ActivityId,
                    WorksheetTemplateId = w.WorksheetTemplateId
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
                    ActivityId = w.ActivityId,
                    WorksheetTemplateId = w.WorksheetTemplateId
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
