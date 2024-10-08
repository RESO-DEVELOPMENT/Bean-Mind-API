﻿using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Videos;
using Bean_Mind.API.Payload.Response.Videos;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Implement
{
    public class VideoService : BaseService<VideoService>, IVideoService
    {
        private readonly GoogleDriveService _driveService;
        public VideoService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<VideoService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, GoogleDriveService driveService) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _driveService = driveService;
        }

        public async Task<CreateNewVideoResponse> CreateNewVideo(CreateNewVideoRequest request, Guid activityId)
        {
            _logger.LogInformation($"Creating new Video with title: {request.Title}");
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var teacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: t => t.AccountId.Equals(accountId) && t.DelFlg == false);

            string url = await _driveService.UploadToGoogleDriveAsync(request.Url);

            var newVideo = new Video()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Url = url,
                SchoolId = account.SchoolId.Value,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                TeacherId = teacher.Id,
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

                newVideo.ActivityId = activityId;
            }

            CreateNewVideoResponse createNewVideoResponse = null;

            await _unitOfWork.GetRepository<Video>().InsertAsync(newVideo);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            if (isSuccessful)
            {
                createNewVideoResponse = new CreateNewVideoResponse()
                {
                    Id = newVideo.Id,
                    Title = newVideo.Title,
                    Description = newVideo.Description,
                    Url = newVideo.Url,
                    ActivityId = newVideo.ActivityId,
                    SchoolId = account.SchoolId,
                    InsDate = newVideo.InsDate,
                    UpdDate = newVideo.UpdDate,
                    DelFlg = newVideo.DelFlg
                };

            }

            return createNewVideoResponse;
        }


        public async Task<IPaginate<GetVideoResponse>> GetListVideo(int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var teacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: t => t.AccountId.Equals(accountId) && t.DelFlg == false);

            var videos = await _unitOfWork.GetRepository<Video>().GetPagingListAsync(
                selector: s => new GetVideoResponse(s.Id, s.Title, s.Description, s.Url, s.ActivityId, s.SchoolId),
                predicate: s => s.DelFlg != true && s.SchoolId.Equals(account.SchoolId) && s.TeacherId.Equals(teacher.Id),
                page: page,
                size: size
                );
            if (videos == null)
            {
                throw new BadHttpRequestException(MessageConstant.VideoMessage.VideoIsEmpty);
            }
            return videos;
        }

        public async Task<GetVideoResponse> GetVideoById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.VideoMessage.VideoNotFound);
            }

            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var teacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: t => t.AccountId.Equals(accountId) && t.DelFlg == false);

            var video = await _unitOfWork.GetRepository<Video>().SingleOrDefaultAsync(
                selector: s => new GetVideoResponse(s.Id, s.Title, s.Description, s.Url, s.ActivityId, s.SchoolId),
                predicate: s => s.Id.Equals(id) && s.DelFlg != true && s.TeacherId.Equals(teacher.Id)
                );
            if (video == null)
            {
                throw new BadHttpRequestException(MessageConstant.VideoMessage.VideoNotFound);
            }
            return video;
        }

        public async Task<bool> DeleteVideo(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.VideoMessage.VideoNotFound);
            }

            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var teacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: t => t.AccountId.Equals(accountId) && t.DelFlg == false);

            var video = await _unitOfWork.GetRepository<Video>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true && s.TeacherId.Equals(teacher.Id));
            if (video == null)
            {
                throw new BadHttpRequestException(MessageConstant.VideoMessage.VideoNotFound);
            }
            video.DelFlg = true;
            video.UpdDate = TimeUtils.GetCurrentSEATime();

            _unitOfWork.GetRepository<Video>().UpdateAsync(video);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }

        public async Task<bool> UpdateVideo(Guid videoId, Guid activityId, UpdateVideoRequest request)
        {
            if (videoId == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.VideoMessage.VideoNotFound);
            }

            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var teacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: t => t.AccountId.Equals(accountId) && t.DelFlg == false);

            var video = await _unitOfWork.GetRepository<Video>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(videoId) && s.DelFlg != true && s.TeacherId.Equals(teacher.Id));
            if (video == null)
            {
                throw new BadHttpRequestException(MessageConstant.VideoMessage.VideoNotFound);
            }

            if (activityId != Guid.Empty)
            {
                var activity = await _unitOfWork.GetRepository<Activity>().SingleOrDefaultAsync(predicate: c => c.Id.Equals(activityId) && c.DelFlg != true);
                if (activity == null)
                {
                    throw new BadHttpRequestException(MessageConstant.ActivityMessage.ActivityNotFound);
                }
                video.ActivityId = activityId;
            }

            video.Title = string.IsNullOrEmpty(request.Title) ? video.Title : request.Title;
            video.Description = string.IsNullOrEmpty(request.Description) ? video.Description : request.Description;
            video.UpdDate = TimeUtils.GetCurrentSEATime();
            if (request.Url != null)
            {
                try
                {
                    video.Url = await _driveService.UploadToGoogleDriveAsync(request.Url);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error uploading file to Google Drive: {ex.Message}");
                    throw new BadHttpRequestException("Error uploading file video to Google Drive.");
                }
            }
            _unitOfWork.GetRepository<Video>().UpdateAsync(video);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }
    }
}
