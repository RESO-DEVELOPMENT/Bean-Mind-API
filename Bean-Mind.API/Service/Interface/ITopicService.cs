using Bean_Mind.API.Payload.Request.Topics;
using Bean_Mind.API.Payload.Response.Topics;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface ITopicService
    {
        public Task<CreateNewTopicResponse> CreateNewTopic(CreateNewTopicRequest request, Guid chapterId);
        public Task<IPaginate<GetTopicResponse>> GetListTopic(int page, int size);
        public Task<GetTopicResponse> GetTopicById(Guid id);
        public Task<bool> DeleteTopic(Guid id);
        public Task<bool> UpdateTopic(Guid topicId, Guid chapterId, UpdateTopicRequest request);

    }
}
