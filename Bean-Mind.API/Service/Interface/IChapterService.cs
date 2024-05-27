using Bean_Mind.API.Payload.Request.Chapters;
using Bean_Mind.API.Payload.Response.Chapters;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface IChapterService
    {
        public Task<CreateNewChapterResponse> CreateNewChapter(CreateNewChapterRequest request, Guid subjectId);
        public Task<IPaginate<GetChapterResponse>> getListChapter(int page, int size);
        public Task<GetChapterResponse> getChapterById(Guid id);
        public Task<bool> UpdateChapter(Guid id, UpdateChapterRequest request, Guid subjectId);
        public Task<bool> RemoveChapter(Guid id);
    }
}
