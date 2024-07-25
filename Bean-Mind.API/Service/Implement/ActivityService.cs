using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Activities;
using Bean_Mind.API.Payload.Response.Activities;
using Bean_Mind.API.Payload.Response.Documents;
using Bean_Mind.API.Payload.Response.Videos;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind.API.Service.Implement
{
    public class ActivityService : BaseService<ActivityService>, IActivityService
    {
        public ActivityService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<ActivityService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateNewActivityResponse> CreateNewActivity(CreateNewActivityRequest request, Guid topicId)
        {
            _logger.LogInformation($"Creating new activity with title: {request.Title}");

            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var activity = new Activity()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                SchoolId = account.SchoolId.Value,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false
            };

            if (topicId != Guid.Empty)
            {
                var topic = await _unitOfWork.GetRepository<Topic>().SingleOrDefaultAsync(
                    predicate: t => t.Id.Equals(topicId) && t.DelFlg != true);

                if (topic == null)
                {
                    throw new BadHttpRequestException(MessageConstant.TopicMessage.TopicNotFound);
                }

                activity.TopicId = topicId;
            }

            await _unitOfWork.GetRepository<Activity>().InsertAsync(activity);
            var isSuccess = await _unitOfWork.CommitAsync() > 0;

            CreateNewActivityResponse createNewActivityResponse = null;
            if (isSuccess)
            {
                createNewActivityResponse = new CreateNewActivityResponse()
                {
                    Id = activity.Id,
                    Title = activity.Title,
                    Description = activity.Description,
                    TopicId = activity.TopicId,
                    SchoolId = account.SchoolId,
                    InsDate = activity.InsDate,
                    UpdDate = activity.UpdDate,
                    DelFlg = activity.DelFlg
                };
            }

            return createNewActivityResponse;
        }


        public async Task<bool> DeleteActivity(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.ActivityMessage.ActivityNotFound);
            }

            var activity = await _unitOfWork.GetRepository<Activity>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(id) && a.DelFlg == false,
                include: a => a.Include(a => a.Videos).Include(a => a.Documents).Include(a => a.WorkSheets).ThenInclude(w => w.WorksheetQuestions));
            if(activity == null)
            {
                throw new BadHttpRequestException(MessageConstant.ActivityMessage.ActivityNotFound);
            }
            foreach(var video in activity.Videos)
            {
                video.DelFlg = true;
                video.UpdDate = TimeUtils.GetCurrentSEATime();
                _unitOfWork.GetRepository<Video>().UpdateAsync(video);
            }
            foreach (var document in activity.Documents)
            {
                document.DelFlg = true;
                document.UpdDate = TimeUtils.GetCurrentSEATime();
                _unitOfWork.GetRepository<Document>().UpdateAsync(document);
            }
            foreach ( var workSheet in activity.WorkSheets)
            {
                foreach (var worksheetQuestion in workSheet.WorksheetQuestions)
                {
                    worksheetQuestion.DelFlg = true;
                    worksheetQuestion.UpdDate = TimeUtils.GetCurrentSEATime();
                    _unitOfWork.GetRepository<WorksheetQuestion>().UpdateAsync(worksheetQuestion);
                }
                workSheet.DelFlg = true;
                workSheet.UpdDate= TimeUtils.GetCurrentSEATime();
                _unitOfWork.GetRepository<WorkSheet>().UpdateAsync(workSheet);
            }
            activity.DelFlg = true;
            activity.UpdDate = TimeUtils.GetCurrentSEATime();
            _unitOfWork.GetRepository<Activity>().UpdateAsync(activity);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }

        public async Task<GetActivityResponse> GetActivityById(Guid id)
        {
            var activity = await _unitOfWork.GetRepository<Activity>().SingleOrDefaultAsync(
                selector: a => new GetActivityResponse
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    TopicId = a.TopicId,
                    SchoolId = a.SchoolId
                },
                predicate: a => a.Id.Equals(id) && a.DelFlg == false);
            if(activity == null)
            {
                throw new BadHttpRequestException(MessageConstant.ActivityMessage.ActivityNotFound);
            }
            return activity;
        }

        public async Task<IPaginate<GetActivityResponse>> GetListActivity(int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var activities = await _unitOfWork.GetRepository<Activity>().GetPagingListAsync(
                selector: a => new GetActivityResponse
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    TopicId = a.TopicId,
                    SchoolId = a.SchoolId
                },
                predicate: a => a.DelFlg == false && a.SchoolId.Equals(account.SchoolId),
                page: page,
                size: size,
                orderBy: a => a.OrderBy(a => a.InsDate)
                );
            return activities;
        }

        public async Task<IPaginate<GetDocumentResponse>> GetListDocument(Guid id, int page, int size)
        {
            var documents = await _unitOfWork.GetRepository<Document>().GetPagingListAsync(
                selector: d => new GetDocumentResponse(d.Id, d.Title, d.Description, d.Url, d.ActivityId, d.SchoolId),
                predicate: d => d.ActivityId.Equals(id) && d.DelFlg == false,
                page: page,
                size: size
                );
            return documents;
        }

        public async Task<IPaginate<GetVideoResponse>> GetListVideo(Guid id, int page, int size)
        {
            var videos = await _unitOfWork.GetRepository<Video>().GetPagingListAsync(
                selector: v => new GetVideoResponse(v.Id, v.Description, v.Title, v.Url, v.ActivityId, v.SchoolId),
                predicate: d => d.ActivityId.Equals(id) && d.DelFlg == false,
                page: page,
                size: size
                );
            return videos;
        }

        public async Task<bool> UpdateActivitỵ(Guid id, UpdateActivityRequest request, Guid topicId)
        {
            if(id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.ActivityMessage.ActivityNotFound);
            }
            var activity = await _unitOfWork.GetRepository<Activity>().SingleOrDefaultAsync(
                predicate: a => a.Id.Equals(id) && a.DelFlg == false);
            if(activity == null)
            {
                throw new BadHttpRequestException(MessageConstant.ActivityMessage.ActivityNotFound);
            }

            if(topicId != Guid.Empty) 
            {
                var topic = await _unitOfWork.GetRepository<Topic>().SingleOrDefaultAsync(
                    predicate: t => t.Id.Equals(topicId) && t.DelFlg == false);
                if (topic == null)
                {
                    throw new BadHttpRequestException(MessageConstant.TopicMessage.TopicNotFound);
                }
                activity.TopicId = topicId;
            }
            activity.Title = string.IsNullOrEmpty(request.Title) ? activity.Title : request.Title;
            activity.Description = string.IsNullOrEmpty(request.Description) ? activity.Description : request.Description;
            activity.UpdDate = TimeUtils.GetCurrentSEATime();
            _unitOfWork.GetRepository<Activity>().UpdateAsync(activity);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
