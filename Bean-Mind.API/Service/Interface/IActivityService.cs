using Bean_Mind.API.Payload.Request.Activities;
using Bean_Mind.API.Payload.Response.Activities;
using Bean_Mind.API.Payload.Response.Documents;
using Bean_Mind.API.Payload.Response.Videos;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface IActivityService
    {
        public Task<CreateNewActivityResponse> CreateNewActivity(CreateNewActivityRequest request, Guid topicId);
        public Task<GetActivityResponse> GetActivityById(Guid id);
        public Task<IPaginate<GetActivityResponse>> GetListActivity(int page, int size);
        public Task<bool> DeleteActivity(Guid id);
        public Task<bool> UpdateActivitỵ(Guid id, UpdateActivityRequest request, Guid topicId);
        public Task<IPaginate<GetVideoResponse>> GetListVideo(Guid id, int page, int size);
        public Task<IPaginate<GetDocumentResponse>> GetListDocument(Guid id, int page, int size);

    }
}
