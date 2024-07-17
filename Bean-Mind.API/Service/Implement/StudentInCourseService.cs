using AutoMapper;
using Bean_Mind.API.Payload.Request.Curriculums;
using Bean_Mind.API.Payload.Response.StudentInCourse;
using Bean_Mind.API.Payload.Response.Students;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Implement
{
    public class StudentInCourseService : BaseService<StudentInCourseService>, IStudentInCourseService
    {
        public StudentInCourseService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<StudentInCourseService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateNewStudentInCourseResponse> CreateStudentInCourse(Guid StudentId, List<Guid> CourseIds)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
            );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");
            var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(StudentId) && s.DelFlg != true
            );
            if(student == null)
                throw new Exception("Student does not exist");
            //Guid accountStudentId = student.AccountId;
            //var AccountStudentId = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            //    predicate: a => a.SchoolId.Equals(account.SchoolId));


            foreach (var CourseId in CourseIds)
            {
                var course = await _unitOfWork.GetRepository<Course>().SingleOrDefaultAsync(
                    predicate: c => c.Id.Equals(CourseIds) && c.DelFlg != true);
                var studentInCourse = new StudentInCourse
                {
                    Id = Guid.NewGuid(),
                    CourseId = CourseId,
                    StudentId = student.Id,
                    InsDate = TimeUtils.GetCurrentSEATime(),
                    UpdDate = TimeUtils.GetCurrentSEATime(),
                    DelFlg = false,
                };

                await _unitOfWork.GetRepository<StudentInCourse>().InsertAsync(studentInCourse);
            }
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            CreateNewStudentInCourseResponse createNewStudentInCourseResponse = null;
            if (isSuccessful)
            {
                createNewStudentInCourseResponse = new CreateNewStudentInCourseResponse
                {
                    StudentId = student.Id,
                    CourseIds = CourseIds
                };
            }
            return createNewStudentInCourseResponse;
        }

        public Task<bool> DeleteStudentInCourse(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IPaginate<GetStudentInCourseResponse>> GetAllStudentInCourses(int page, int size)
        {
            throw new NotImplementedException();
        }

        public Task<IPaginate<GetStudentInCourseResponse>> GetStudentInCourseByCourse(Guid courseId, int page, int size)
        {
            throw new NotImplementedException();
        }

        public Task<IPaginate<GetStudentInCourseResponse>> GetStudentInCourseByStudent(Guid studentId, int page, int size)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStudentInCourse(Guid id, Guid StudentId, List<Guid> CourseId)
        {
            throw new NotImplementedException();
        }
    }
}
