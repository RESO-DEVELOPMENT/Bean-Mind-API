using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Courses;
using Bean_Mind.API.Payload.Response.Courses;
using Bean_Mind.API.Payload.Response.Subjects;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Enums;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using Microsoft.OpenApi.Extensions;

namespace Bean_Mind.API.Service.Implement
{
    public class CourseService : BaseService<CourseService>, ICourseService
    {
        public CourseService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<CourseService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }
        public async Task<CreateNewCourseResponse> CreateNewCourse(CreateNewCourseRequest createNewCourseRequest)
        {
            _logger.LogInformation($"Creating new Course with title: {createNewCourseRequest.Title}");

            var curriculum = await _unitOfWork.GetRepository<Curriculum>().SingleOrDefaultAsync(predicate: c => c.Id == createNewCourseRequest.CurriculumId && c.DelFlg != true);
            if (curriculum == null)
            {
                _logger.LogError($"Curriculum with id {createNewCourseRequest.CurriculumId} not found.");
                return null;
            }

            var newCourse = new Course
            {
                Id = Guid.NewGuid(),
                Title = createNewCourseRequest.Title,
                Description = createNewCourseRequest.Description,
                StartDate = createNewCourseRequest.StartDate,
                EndDate = createNewCourseRequest.EndDate,
                CurriculumId = createNewCourseRequest.CurriculumId,
                Status = (int)(createNewCourseRequest.Status),
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false
            };

            await _unitOfWork.GetRepository<Course>().InsertAsync(newCourse);
            bool  isSuccessful = await _unitOfWork.CommitAsync() > 0;

            // return isSuccessful ? _mapper.Map<CreateNewCourseResponse>(newCourse) : null;

            CreateNewCourseResponse Response = null;
            if (isSuccessful) {
                Response = new CreateNewCourseResponse()
                {
                    Id = Guid.NewGuid(),
                    Title = newCourse.Title,
                    Description = newCourse.Description,
                    StartDate = newCourse.StartDate,
                    EndDate = newCourse.EndDate,
                    Status = newCourse.Status,
                    CurriculumId = newCourse.CurriculumId.Value,
                    InsDate = newCourse.InsDate,
                    UpdDate = newCourse.UpdDate,
                    DelFlg = false

                };
            }
            return Response;
        }

        public async Task<bool> DeleteCourse(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage.CourseNotFound);
            }
            var course  = await _unitOfWork.GetRepository<Course>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(Id) && s.DelFlg != true);
            if (course == null)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage
                    .CourseNotFound);
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
            var curriculums = await _unitOfWork.GetRepository<Course>().SingleOrDefaultAsync(
             selector: s => new GetCourseResponse
             {
                 Id = s.Id,
                 Title = s.Title,
                 Description = s.Description,
                 StartDate = s.StartDate,
                 EndDate = s.EndDate,
                 Status = s.Status,
                 CurriculumId = s.CurriculumId.Value,
                 InsDate = s.InsDate,
                 UpdDate = s.UpdDate,
                 DelFlg = s.DelFlg,
             },
             predicate: x => x.Id == Id && x.DelFlg != true
             );

            if (curriculums == null)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage.CourseNotFound);
            }
            return curriculums;
        }

       

        public async Task<IPaginate<GetCourseResponse>> GetListCourse(int page, int size)
        {
            var curriculums = await _unitOfWork.GetRepository<Course>().GetPagingListAsync(
              selector: s => new GetCourseResponse
              {
                  Id = s.Id,
                  Title = s.Title,
                  Description = s.Description,
                  StartDate = s.StartDate,
                  EndDate = s.EndDate,
                  Status = s.Status,
                  CurriculumId = s.CurriculumId.Value,
                  InsDate = s.InsDate,
                  UpdDate = s.UpdDate,
                  DelFlg = s.DelFlg,
              },
              predicate: x => x.DelFlg == false,
              size: size,
              page: page);

            return curriculums;
        }

        

        public async Task<bool> UpdateCourse(Guid Id, UpdateCourseRequest updateCourseRequest, Guid curriculumId)
        {
            if (Id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage.CourseNotFound);
            }
            var course  = await _unitOfWork.GetRepository<Course>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(Id) && s.DelFlg != true);
            if (course == null)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage.CourseNotFound);
            }
            if (curriculumId != Guid.Empty)
            {
                Curriculum curriculum = await _unitOfWork.GetRepository<Curriculum>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(curriculumId) && s.DelFlg !=true);
                if (curriculum == null)
                {
                    throw new BadHttpRequestException(MessageConstant.CurriculumMessage.CurriculumNotFound);
                }
                course.CurriculumId = curriculumId;
            }


            course.Title = string.IsNullOrEmpty(updateCourseRequest.Title) ? course.Title : updateCourseRequest.Title;
            course.Description = string.IsNullOrEmpty(updateCourseRequest.Description) ? course.Description : updateCourseRequest.Description;
            course.Status = updateCourseRequest.Status.HasValue ? (int)updateCourseRequest.Status.Value : course.Status;
            course.StartDate = (updateCourseRequest.StartDate.HasValue && updateCourseRequest.StartDate != DateTime.MinValue) ? updateCourseRequest.StartDate.Value : course.StartDate;
            course.EndDate = (updateCourseRequest.EndDate.HasValue && updateCourseRequest.EndDate != DateTime.MinValue) ? updateCourseRequest.EndDate.Value : course.EndDate;
            course.UpdDate = TimeUtils.GetCurrentSEATime();

            DateTime date;

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

            var subjects = await _unitOfWork.GetRepository<Subject>().GetPagingListAsync(
                selector: s => new GetSubjectResponse(s.Id, s.Title, s.Description),
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


    }
}
