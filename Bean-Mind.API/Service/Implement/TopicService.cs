using AutoMapper;
using Azure;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Topics;
using Bean_Mind.API.Payload.Response.Topics;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind.API.Service.Implement
{
    public class TopicService : BaseService<TopicService>, ITopicService
    {
        public TopicService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<TopicService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateNewTopicResponse> CreateNewTopic(CreateNewTopicRequest Request, Guid chapterId)
        {
            _logger.LogInformation($"Creating new Topic with title: {Request.Title}");

            var newTopic = new Topic
            {
                Id = Guid.NewGuid(),
                Title = Request.Title,
                Description = Request.Description,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false
            };

            if (chapterId != Guid.Empty)
            {
                var chapter = await _unitOfWork.GetRepository<Chapter>().SingleOrDefaultAsync(predicate: c => c.Id.Equals(chapterId) && c.DelFlg != true);
                if (chapter == null)
                {
                    _logger.LogError($"Chapter with id {chapterId} not found.");
                    return null;
                }

                newTopic.ChapterId = chapterId;
            }

            await _unitOfWork.GetRepository<Topic>().InsertAsync(newTopic);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            CreateNewTopicResponse response = null;
            if (isSuccessful)
            {
                response = new CreateNewTopicResponse()
                {
                    Id = newTopic.Id,
                    Title = newTopic.Title,
                    Description = newTopic.Description,
                    ChapterId = newTopic.ChapterId,
                    InsDate = newTopic.InsDate,
                    UpdDate = newTopic.UpdDate,
                    DelFlg = newTopic.DelFlg
                };
            }

            return response;
        }


        public async Task<bool> DeleteTopic(Guid id)
        {
            var topic = await _unitOfWork.GetRepository<Topic>().SingleOrDefaultAsync(
                predicate: t => t.Id.Equals(id) && t.DelFlg != true);
            if(topic == null)
                throw new BadHttpRequestException(MessageConstant.TopicMessage.ListIsEmpty);
            topic.DelFlg = true;
            topic.UpdDate = TimeUtils.GetCurrentSEATime();
            _unitOfWork.GetRepository<Topic>().UpdateAsync(topic);
            bool isSuccess = await _unitOfWork.CommitAsync() > 0;
            return isSuccess;
        }

        public async Task<IPaginate<GetTopicResponse>> GetListTopic(int page, int size)
        {
            var topics = await _unitOfWork.GetRepository<Topic>().GetPagingListAsync(
                    selector: t => new GetTopicResponse(t.Id, t.Title, t.Description, t.ChapterId),
                    include: t => t.Include(t => t.Chapter),
                    predicate: t => t.DelFlg == false,
                    page: page,
                    size: size);
            return topics;
        }

        public async Task<GetTopicResponse> GetTopicById(Guid id)
        {
            var topic = await _unitOfWork.GetRepository<Topic>().SingleOrDefaultAsync(
                selector: t => new GetTopicResponse(t.Id, t.Title, t.Description, t.ChapterId),
                predicate: t => t.Id.Equals(id) && t.DelFlg != true,
                include: t => t.Include(t => t.Chapter)
            );
                
            if(topic == null)
                throw new BadHttpRequestException(MessageConstant.TopicMessage.TopicNotFound);

            return topic;
        }

        public async Task<bool> UpdateTopic(Guid topicId, Guid chapterId, UpdateTopicRequest request)
        {
            if (topicId == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.TopicMessage.TopicNotFound);
            }
            var topic = await _unitOfWork.GetRepository<Topic>().SingleOrDefaultAsync(predicate: t => t.Id.Equals(topicId) && t.DelFlg != true);
            if (topic == null)
            {
                throw new BadHttpRequestException(MessageConstant.TopicMessage.TopicNotFound);
            }
            if (chapterId != Guid.Empty)
            {
                Chapter chapter = await _unitOfWork.GetRepository<Chapter>().SingleOrDefaultAsync(predicate: c => c.Id.Equals(chapterId) && c.DelFlg != true);
                if (chapter == null)
                {
                    throw new BadHttpRequestException(MessageConstant.SchoolMessage.SchoolNotFound);
                }
                topic.ChapterId = chapterId;
            }

            topic.Title = string.IsNullOrEmpty(request.Title.ToString()) ? topic.Title : request.Title;
            topic.Description = string.IsNullOrEmpty(request.Description) ? topic.Description : request.Description;
            topic.UpdDate = TimeUtils.GetCurrentSEATime();

            _unitOfWork.GetRepository<Topic>().UpdateAsync(topic);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
