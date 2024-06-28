using Bean_Mind_Data.Paginate;
using Bean_Mind.API.Payload.Request.TeacherTeachables;
using Bean_Mind.API.Payload.Response.TeacherTeachables;

namespace Bean_Mind.API.Service.Interface
{
    public interface ITeacherTeachableService
    {
        Task<GetTeacherTeachableResponse> CreateTeacherTeachable(CreateNewTeacherTeachableRequest request);

        Task<IPaginate<GetTeacherTeachableResponse>> GetAllTeacherTeachables(int page, int size);
        Task<IPaginate<GetTeacherTeachableResponse>> GetTeacherTeachablesByTeacher(Guid teacherId, int page, int size);
        Task<IPaginate<GetTeacherTeachableResponse>> GetTeacherTeachablesBySubject(Guid subjectId, int page, int size);
        Task<bool> UpdateTeacherTeachable(Guid id, UpdateTeacherTeachableRequest request);
        Task<bool> RemoveTeacherTeachable(Guid id);

    }
}
