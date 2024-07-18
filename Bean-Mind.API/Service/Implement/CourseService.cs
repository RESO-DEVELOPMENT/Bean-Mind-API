using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Courses;
using Bean_Mind.API.Payload.Request.Curriculums;
using Bean_Mind.API.Payload.Response.Courses;
using Bean_Mind.API.Payload.Response.StudentInCourse;
using Bean_Mind.API.Payload.Response.Subjects;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Implement
{
    public class CourseService : BaseService<CourseService>, ICourseService
    {
        public CourseService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<CourseService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }
        public async Task<CreateNewCourseResponse> CreateNewCourse(CreateNewCourseRequest createNewCourseRequest, Guid curriculumId)
        {
            _logger.LogInformation($"Creating new Course with title: {createNewCourseRequest.Title}");

            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");
            if (createNewCourseRequest.StartDate < DateTime.UtcNow.Date)
                throw new Exception("StartDate cannot be in the past");

            if (createNewCourseRequest.EndDate < DateTime.UtcNow.Date)
                throw new Exception("EndDate cannot be in the past");

            if (createNewCourseRequest.StartDate > createNewCourseRequest.EndDate)
                throw new Exception("StartDate must be before EndDate");
            var newCourse = new Course
            {
                Id = Guid.NewGuid(),
                Title = createNewCourseRequest.Title,
                CourseCode = createNewCourseRequest.CourseCode,
                Description = createNewCourseRequest.Description,
                StartDate = createNewCourseRequest.StartDate,
                EndDate = createNewCourseRequest.EndDate,
                Status = (int)(createNewCourseRequest.Status),
                SchoolId = account.SchoolId.Value,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false
            };

        if (curriculumId != Guid.Empty)
            {
                var curriculum = await _unitOfWork.GetRepository<Curriculum>().SingleOrDefaultAsync(predicate: c => c.Id == curriculumId && c.DelFlg != true);
                if (curriculum == null)
                {
                    _logger.LogError($"Curriculum with id {curriculumId} not found.");
                    return null;
                }

                // Assign curriculumId to the new course
                newCourse.CurriculumId = curriculumId;
            }

            // Insert new course into the database
            await _unitOfWork.GetRepository<Course>().InsertAsync(newCourse);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            // If the course creation was successful, construct the response
            CreateNewCourseResponse response = null;
            if (isSuccessful)
            {
                response = new CreateNewCourseResponse()
                {
                    Id = newCourse.Id,
                    Title = newCourse.Title,
                    CourseCode = newCourse.CourseCode,
                    Description = newCourse.Description,
                    StartDate = newCourse.StartDate,
                    EndDate = newCourse.EndDate,
                    Status = newCourse.Status,
                    CurriculumId = newCourse.CurriculumId,
                    SchoolId = newCourse.SchoolId,
                    InsDate = newCourse.InsDate,
                    UpdDate = newCourse.UpdDate,
                    DelFlg = newCourse.DelFlg
                };
            }

            return response;
        }


        public async Task<bool> DeleteCourse(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage.CourseNotFound);
            }
            var course = await _unitOfWork.GetRepository<Course>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(Id) && s.DelFlg != true);
            if (course == null)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage
                    .CourseNotFound);
            }
            var studentInCourses = await _unitOfWork.GetRepository<StudentInCourse>().GetListAsync(predicate: s => s.CourseId.Equals(Id) && s.DelFlg != true);
            foreach (var studentInCourse in studentInCourses)
            {
                studentInCourse.DelFlg = true;
                studentInCourse.UpdDate = TimeUtils.GetCurrentSEATime();
                _unitOfWork.GetRepository<StudentInCourse>().UpdateAsync(studentInCourse);
            }
            var subjects = await _unitOfWork.GetRepository<Subject>().GetListAsync(
                        predicate: s => s.CourseId.Equals(Id) && s.DelFlg == false);
            foreach (var subject in subjects)
            {
                var chapters = await _unitOfWork.GetRepository<Chapter>().GetListAsync(
                    predicate: c => c.SubjectId.Equals(subject.Id) && c.DelFlg == false);
                foreach (var chapter in chapters)
                {
                    var topics = await _unitOfWork.GetRepository<Topic>().GetListAsync(
                        predicate: t => t.ChapterId.Equals(chapter.Id) && t.DelFlg == false);
                    foreach (var topic in topics)
                    {
                        topic.DelFlg = true;
                        topic.UpdDate = TimeUtils.GetCurrentSEATime();
                        _unitOfWork.GetRepository<Topic>().UpdateAsync(topic);
                    }
                    chapter.DelFlg = true;
                    chapter.UpdDate = TimeUtils.GetCurrentSEATime();
                    _unitOfWork.GetRepository<Chapter>().UpdateAsync(chapter);
                }
                subject.DelFlg = true;
                subject.UpdDate = TimeUtils.GetCurrentSEATime();
                _unitOfWork.GetRepository<Subject>().UpdateAsync(subject);
            }

            course.DelFlg = true;
            course.UpdDate = TimeUtils.GetCurrentSEATime();
            _unitOfWork.GetRepository<Course>().UpdateAsync(course);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<GetCourseResponse> GetCourseById(Guid Id)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");
            var course = await _unitOfWork.GetRepository<Course>().SingleOrDefaultAsync(
             selector: s => new GetCourseResponse
             {
                 Id = s.Id,
                 Title = s.Title,
                 CourseCode = s.CourseCode,
                 Description = s.Description,
                 StartDate = s.StartDate,
                 EndDate = s.EndDate,
                 Status = s.Status,
                 CurriculumId = s.CurriculumId,
                 SchoolId = s.SchoolId                 
             },
             predicate: x => x.Id.Equals(Id) && x.DelFlg != true && x.SchoolId.Equals(account.SchoolId)
             );

            if (course == null)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage.CourseNotFound);
            }
            return course;
        }

        public async Task<IPaginate<GetCourseResponse>> GetListCourse(int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var courses = await _unitOfWork.GetRepository<Course>().GetPagingListAsync(
              selector: s => new GetCourseResponse
              {
                  Id = s.Id,
                  Title = s.Title,
                  CourseCode = s.CourseCode,
                  Description = s.Description,
                  StartDate = s.StartDate,
                  EndDate = s.EndDate,
                  Status = s.Status,
                  CurriculumId = s.CurriculumId,
                  SchoolId = s.SchoolId,
              },
            predicate: x => x.DelFlg == false && x.SchoolId.Equals(account.SchoolId),
              size: size,
              page: page);

            return courses;
        }

        public async Task<bool> UpdateCourse(Guid Id, UpdateCourseRequest updateCourseRequest, Guid curriculumId)
        {
            if (Id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage.CourseNotFound);
            }
            var course = await _unitOfWork.GetRepository<Course>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(Id) && s.DelFlg != true);
            if (course == null)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage.CourseNotFound);
            }
            if (curriculumId != Guid.Empty)
            {
                Curriculum curriculum = await _unitOfWork.GetRepository<Curriculum>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(curriculumId) && s.DelFlg != true);
                if (curriculum == null)
                {
                    throw new BadHttpRequestException(MessageConstant.CurriculumMessage.CurriculumNotFound);
                }
                course.CurriculumId = curriculumId;
            }

            course.Title = string.IsNullOrEmpty(updateCourseRequest.Title) ? course.Title : updateCourseRequest.Title;
            course.Description = string.IsNullOrEmpty(updateCourseRequest.Description) ? course.Description : updateCourseRequest.Description;
            course.Status = updateCourseRequest.Status.HasValue ? (int)updateCourseRequest.Status.Value : course.Status;
            course.CourseCode = string.IsNullOrEmpty(updateCourseRequest.CourseCode) ? course.CourseCode : updateCourseRequest.CourseCode;
            course.StartDate = updateCourseRequest.StartDate ?? course.StartDate;
            course.EndDate = updateCourseRequest.EndDate ?? course.EndDate;
            course.UpdDate = TimeUtils.GetCurrentSEATime();

            _unitOfWork.GetRepository<Course>().UpdateAsync(course);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
        public async Task<IPaginate<GetSubjectResponse>> GetListSubjectsByCourseId(Guid id, int page, int size)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage.CourseNotFound);
            }
            var course = await _unitOfWork.GetRepository<Course>().SingleOrDefaultAsync(
             predicate: x => x.Id == id && x.DelFlg != true
             );

            if (course == null)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage.CourseNotFound);
            }

            var subjects = await _unitOfWork.GetRepository<Subject>().GetPagingListAsync(
                selector: s => new GetSubjectResponse(s.Id, s.Title, s.SubjectCode,s.Description, s.CourseId, s.SchoolId),
                predicate: s => s.CourseId.Equals(id) && s.DelFlg != true,
                page: page,
                size: size
            );

            if (subjects == null)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectsIsEmpty);
            }

            return subjects;
        }


        public async Task<IPaginate<GetCourseResponse>> GetListCourseByTitle(string title, int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");
            var courses = await _unitOfWork.GetRepository<Course>().GetPagingListAsync(
                selector: c => new GetCourseResponse
                {
                    Id = c.Id,
                    Title = c.Title,
                    CourseCode = c.CourseCode,
                    Description = c.Description,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Status = c.Status,
                    CurriculumId = c.CurriculumId,
                    SchoolId = c.SchoolId
                },
                predicate: c => c.Title.Contains(title) && c.DelFlg == false && c.SchoolId.Equals(account.SchoolId),
                page: page,
                size: size
                );
            if (courses == null)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage.CoursesIsEmpty);
            }
            return courses;
        }

        public async Task<IPaginate<GetCourseResponse>> GetListCourseByCode(string code, int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");
            var courses = await _unitOfWork.GetRepository<Course>().GetPagingListAsync(
                selector: c => new GetCourseResponse
                {
                    Id = c.Id,
                    Title = c.Title,
                    CourseCode = c.CourseCode,
                    Description = c.Description,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Status = c.Status,
                    CurriculumId = c.CurriculumId,
                    SchoolId = c.SchoolId
                },
                predicate: c => c.CourseCode.Contains(code) && c.DelFlg == false && c.SchoolId.Equals(account.SchoolId),
                page: page,
                size: size
                );
            if (courses == null)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage.CoursesIsEmpty);
            }
            return courses;
        }

        public async Task<IPaginate<GetStudentInCourseResponse>> GetStudentInCourseByCourse(Guid id, int page, int size)
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
                    CourseId = s.CourseId
                },
                predicate: s => s.CourseId.Equals(id) && s.DelFlg != true && s.Student.Account.SchoolId.Equals(account.SchoolId),
                page: page,
                size: size);
            return studentInCourses;
        }
    }
}
