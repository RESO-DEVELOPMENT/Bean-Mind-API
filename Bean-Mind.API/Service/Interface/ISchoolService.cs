﻿
using Bean_Mind.API.Payload.Request.Schools;
using Bean_Mind.API.Payload.Response.Curriculums;
using Bean_Mind.API.Payload.Response.QuestionLevels;
using Bean_Mind.API.Payload.Response.Schools;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface ISchoolService
    {
        public Task<CreateNewSchoolResponse> CreateNewSchool(CreateNewSchoolRequest createNewSchoolRequest);
        public Task<IPaginate<GetSchoolResponse>> getListSchool(int page, int size);

        public Task<GetSchoolResponse> getSchoolById(Guid Id);
        public Task<bool> deleteSchool(Guid Id);
        public Task<bool> updateSchool(UpdateSchoolRequest request, Guid Id);
        public Task<IPaginate<GetCurriculumResponse>> GetListCurriculum(Guid id, int page, int size);
        public Task<IPaginate<GetQuestionLevelResponse>> GetListQuestionLevel(Guid id, int page, int size);
    }
}
