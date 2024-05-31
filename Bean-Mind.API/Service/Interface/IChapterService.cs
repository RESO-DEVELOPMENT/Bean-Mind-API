using Bean_Mind.API.Payload.Request.Chapters;
using Bean_Mind.API.Payload.Response.Chapters;
using Bean_Mind.API.Payload.Response.Topics;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface IChapterService
    {
        public Task<CreateNewChapterResponse> CreateNewChapter(CreateNewChapterRequest request, Guid subjectId);
        public Task<IPaginate<GetChapterResponse>> GetListChapter(int page, int size);
        public Task<IPaginate<GetTopicResponse>> GetListTopic(Guid id, int page, int size);
        public Task<GetChapterResponse> GetChapterById(Guid id);
        public Task<bool> UpdateChapter(Guid id, UpdateChapterRequest request, Guid subjectId);
        public Task<bool> RemoveChapter(Guid id);
    }
}
