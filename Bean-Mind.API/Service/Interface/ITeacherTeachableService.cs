using Bean_Mind_Data.Paginate;
using Bean_Mind.API.Payload.Request.TeacherTeachables;
using Bean_Mind.API.Payload.Response.TeacherTeachables;

namespace Bean_Mind.API.Service.Interface
{
    public interface ITeacherTeachableService
    {
        Task<GetTeacherTeachableResponse> CreateTeacherTeachable(CreateNewTeacherTeachableRequest request);

        Task<IPaginate<GetTeacherTeachableResponse>> GetAllTeacherTeachables(int page, int size);
        Task<ICollection<GetTeacherTeachableResponse>> GetTeacherTeachablesByTeacher(Guid teacherId);
        Task<ICollection<GetTeacherTeachableResponse>> GetTeacherTeachablesBySubject(Guid subjectId);
        Task<bool> UpdateTeacherTeachable(Guid teacherId, Guid subjectId, UpdateTeacherTeachableRequest request);
        Task<bool> RemoveTeacherTeachable(Guid id);

    }
}
