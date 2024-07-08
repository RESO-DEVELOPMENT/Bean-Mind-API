﻿using Bean_Mind.API.Payload.Request.Students;
using Bean_Mind.API.Payload.Response.Students;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface IStudentService
    {
        public Task<CreateNewStudentResponse> CreateNewStudent(CreateNewStudentRequest request, Guid parentId, Guid courseId);
        public Task<IPaginate<GetStudentResponse>> getListStudent(int page, int size);
        public Task<GetStudentResponse> getStudentById(Guid id);
        Task<bool> UpdateStudent(Guid id, UpdateStudentRequest request, Guid parentId, Guid courseId);
        Task<bool> RemoveStudent(Guid id);
    }
}
