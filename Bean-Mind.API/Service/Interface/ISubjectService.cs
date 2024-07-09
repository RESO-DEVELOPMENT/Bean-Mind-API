using Bean_Mind.API.Payload.Request.Subjects;
using Bean_Mind.API.Payload.Response.Chapters;
using Bean_Mind.API.Payload.Response.Courses;
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
        public Task<IPaginate<GetChapterResponse>> GetListChapter(Guid id, int page, int size);
        public Task<IPaginate<GetSubjectResponse>> GetListSubjectByTitle(string title, int page, int size);
        public Task<IPaginate<GetSubjectResponse>> GetListSubjectByCode(string code, int page, int size);
    }
}
