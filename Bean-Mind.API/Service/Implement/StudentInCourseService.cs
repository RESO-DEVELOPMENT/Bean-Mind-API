using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Curriculums;
using Bean_Mind.API.Payload.Response.StudentInCourse;
using Bean_Mind.API.Payload.Response.Students;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Drawing;

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
                predicate: s => s.Id.Equals(StudentId) && s.Account.SchoolId.Equals(account.SchoolId) && s.DelFlg != true
            );
            if(student == null)
                throw new Exception("Student does not exist");

            foreach (var CourseId in CourseIds)
            {
                var course = await _unitOfWork.GetRepository<Course>().SingleOrDefaultAsync(
                    predicate: c => c.Id.Equals(CourseIds) && c.SchoolId.Equals(account.SchoolId) && c.DelFlg != true);
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

        public async Task<bool> DeleteStudentInCourse(Guid id)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var studentInCourse = await _unitOfWork.GetRepository<StudentInCourse>().SingleOrDefaultAsync(
                predicate: s => s.DelFlg != true && s.Course.SchoolId.Equals(account.SchoolId) && s.Id.Equals(id)
                );
            if(studentInCourse == null)
                throw new BadHttpRequestException(MessageConstant.StudentInCourseMessage.NotFound);
            studentInCourse.DelFlg = true;
            studentInCourse.UpdDate = TimeUtils.GetCurrentSEATime();
            _unitOfWork.GetRepository<StudentInCourse>().UpdateAsync(studentInCourse);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<IPaginate<GetStudentInCourseResponse>> GetAllStudentInCourses(int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var studentInCourses = await _unitOfWork.GetRepository<StudentInCourse>().GetPagingListAsync(
                selector: s => new GetStudentInCourseResponse
                {
                    Id = s.Id,
                    StudentId = s.StudentId,
                    CourseId = s.CourseId,
                    StudentName = s.Student.FirstName + " " + s.Student.LastName                },
                predicate: s => s.DelFlg != true && s.Course.SchoolId.Equals(account.SchoolId),
                page: page,
                size: size
                );
            return studentInCourses;
        }
        public async Task<bool> UpdateStudentInCourse(Guid id, Guid StudentId, Guid CourseId)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");
            var studentInCourse = await _unitOfWork.GetRepository<StudentInCourse>().SingleOrDefaultAsync(
                predicate: s => s.DelFlg != true && s.Course.SchoolId.Equals(account.SchoolId) && s.Id.Equals(id)
                );
            if (studentInCourse == null)
                throw new BadHttpRequestException(MessageConstant.StudentInCourseMessage.NotFound);
            var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(StudentId) && s.Account.SchoolId.Equals(account.SchoolId) && s.DelFlg != true
            );
            if (student == null)
                throw new Exception("Student does not exist");
            var course = await _unitOfWork.GetRepository<Course>().SingleOrDefaultAsync(
                predicate: c => c.Id.Equals(CourseId) && c.SchoolId.Equals(account.SchoolId) && c.DelFlg != true
            );
            if(course == null)
                throw new Exception("Course does not exist");
            studentInCourse.UpdDate = TimeUtils.GetCurrentSEATime();
            studentInCourse.StudentId = student.Id;
            studentInCourse.CourseId = course.Id;
            _unitOfWork.GetRepository<StudentInCourse>().UpdateAsync(studentInCourse);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
