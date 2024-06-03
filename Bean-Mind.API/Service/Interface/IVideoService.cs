using Bean_Mind.API.Payload.Request.Videos;
using Bean_Mind.API.Payload.Response.Videos;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface IVideoService
    {
        public Task<CreateNewVideoResponse> CreateNewVideo(CreateNewVideoRequest request, Guid activityId);
        public Task<IPaginate<GetVideoResponse>> GetListVideo(int page, int size);
        public Task<GetVideoResponse> GetVideoById(Guid id);
        public Task<bool> DeleteVideo(Guid id);
        public Task<bool> UpdateVideo(Guid videoId, Guid activtyId, UpdateVideoRequest request);
    }
}
