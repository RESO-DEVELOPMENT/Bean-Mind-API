﻿using Bean_Mind.API.Payload.Request.Student;
using Bean_Mind.API.Payload.Request.Teacher;
using Bean_Mind.API.Payload.Response.Student;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface IStudentService
    {
        public Task<CreateNewStudentResponse> CreateNewStudent(CreateNewStudentRequest request, Guid schoolId, Guid parentId);
        public Task<IPaginate<GetStudentResponse>> getListStudent(int page, int size);
        public Task<GetStudentResponse> getStudentById(Guid id);
        Task<bool> UpdateStudent(Guid id, CreateNewStudentRequest request, Guid schoolId, Guid parentId);
        Task<bool> RemoveStudent(Guid id);
    }
}
