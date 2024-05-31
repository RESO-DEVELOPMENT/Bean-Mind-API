using Bean_Mind.API.Payload.Request.Curriculums;
using Bean_Mind.API.Payload.Response.Chapters;
using Bean_Mind.API.Payload.Response.Courses;
using Bean_Mind.API.Payload.Response.Curriculums;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface ICurriculumService
    {
        public Task<CreateNewCurriculumResponse> CreateNewCurriculum(CreateNewCurriculumRequest createNewCurriculumRequest);
        public Task<IPaginate<GetCurriculumResponse>> getListCurriculum(int page, int size);

        public Task<GetCurriculumResponse> getCurriculumById(Guid Id);
        public Task<bool> deleteCurriculum(Guid Id);
        public Task<bool> updateCurriculum(Guid Id,UpdateCurriculumRequest updateCurriculumRequest
            , Guid SchoolId);
        public Task<IPaginate<GetCourseResponse>> GetListCourses(Guid id, int page, int size);
    }
}
