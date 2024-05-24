using Bean_Mind.API.Payload.Request.Parent;
using Bean_Mind.API.Payload.Request.Teacher;
using Bean_Mind.API.Payload.Response.Parent;
using Bean_Mind.API.Payload.Response.Teacher;
using Bean_Mind_Data.Paginate;
using System.Threading.Tasks;

namespace Bean_Mind.API.Service.Interface
{
    public interface IParentService
    {
        public Task<CreateNewParentResponse> AddParent(CreateNewParentResquest createNewParentRequest);
        public Task<IPaginate<ParentResponse>> GetAllParents(int page, int size);
        public Task<ParentResponse> GetParentById(Guid parentId);
        public Task<bool> UpdateParent(Guid id, UpdateParentRequest request);
        public Task<bool> RemoveParent(Guid id);
    }
}
