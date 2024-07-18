using Bean_Mind.API.Payload.Request.Students;
using Bean_Mind.API.Payload.Response.StudentInCourse;
using Bean_Mind.API.Payload.Response.Students;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface IStudentService
    {
        public Task<CreateNewStudentResponse> CreateNewStudent(CreateNewStudentRequest request, String parentPhone);
        public Task<IPaginate<GetStudentResponse>> getListStudent(int page, int size);
        public Task<GetStudentResponse> getStudentById(Guid id);
        Task<bool> UpdateStudent(Guid id, UpdateStudentRequest request, Guid parentId);
        Task<bool> RemoveStudent(Guid id);
        Task<IPaginate<GetStudentInCourseResponse>> GetStudentInCourseByStudent(Guid id, int page, int size);
    }
}
