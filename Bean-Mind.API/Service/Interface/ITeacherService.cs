using Bean_Mind.API.Payload.Request.Teachers;
using Bean_Mind.API.Payload.Response.Teachers;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface ITeacherService
    {
        public Task<CreateNewTeacherResponse> CreateTeacher(CreateNewTeacherResquest createNewTeacherRequest, Guid schoolId);
        Task<IPaginate<GetTeacherResponse>> GetAllTeachers(int page, int size);
        Task<GetTeacherResponse> GetTeacherById(Guid teacherId);
        Task<bool> UpdateTeacher(Guid id, UpdateTecherRequest  request);
        Task<bool> RemoveTeacher(Guid teacherId);
    }
}
