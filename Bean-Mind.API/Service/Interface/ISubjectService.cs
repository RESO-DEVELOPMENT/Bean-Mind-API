using Bean_Mind.API.Payload.Request.Subjects;
using Bean_Mind.API.Payload.Response.Subjects;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface ISubjectService
    {
        public Task<CreateNewSubjectResponse> CreateNewSubject(CreateNewSubjectRequest request, Guid courseId);
        public Task<IPaginate<GetSubjectResponse>> getListSubject(int page, int size);
        public Task<GetSubjectResponse> getSubjectById(Guid id);
        public Task<bool> UpdateSubject(Guid id, UpdateSubjectRequest request, Guid courseId);
        public Task<bool> RemoveSubject(Guid id);
    }
}
