using Bean_Mind.API.Payload.Request.TeacherTeachables;
using Bean_Mind.API.Payload.Response.StudentInCourse;
using Bean_Mind.API.Payload.Response.TeacherTeachables;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface IStudentInCourseService
    {
        Task<CreateNewStudentInCourseResponse> CreateStudentInCourse(Guid StudentId, List<Guid> CourseIds);

        Task<IPaginate<GetStudentInCourseResponse>> GetAllStudentInCourses(int page, int size);
        Task<bool> UpdateStudentInCourse(Guid id, Guid StudentId, Guid CourseId);
        Task<bool> DeleteStudentInCourse(Guid id);
    }
}
