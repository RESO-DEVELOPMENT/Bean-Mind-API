using Bean_Mind.API.Payload.Request.Courses;
using Bean_Mind.API.Payload.Response.Courses;
using Bean_Mind.API.Payload.Response.Subjects;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface ICourseService
    {
        Task<CreateNewCourseResponse> CreateNewCourse(CreateNewCourseRequest createNewCourseRequest);
        Task<GetCourseResponse> GetCourseById(Guid id);
        Task<IPaginate<GetCourseResponse>> GetListCourse(int page, int size);
        Task<bool> UpdateCourse(Guid id, UpdateCourseRequest updateCourseRequest, Guid curriculumId);
        Task<bool> DeleteCourse(Guid id);
        public Task<IPaginate<GetSubjectResponse>> GetListSubjectsByCourseId(Guid id, int page, int size);

    }
}
