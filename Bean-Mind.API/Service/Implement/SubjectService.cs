using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Subjects;
using Bean_Mind.API.Payload.Response.Chapters;
using Bean_Mind.API.Payload.Response.Students;
using Bean_Mind.API.Payload.Response.Subjects;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Enums;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind.API.Service.Implement
{
    public class SubjectService : BaseService<SubjectService>, ISubjectService
    {
        public SubjectService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<SubjectService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateNewSubjectResponse> CreateNewSubject(CreateNewSubjectRequest request, Guid courseId)
        {
            _logger.LogInformation($"Create new Subject with {request.Title}");
            if (courseId == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage.CourseNotFound);
            }
            Course course = await _unitOfWork.GetRepository<Course>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(courseId) && s.DelFlg != true);
            if (course == null)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage.CourseNotFound);
            }

            Subject newSubject = new Subject()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                DelFlg = false,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                CourseId = courseId,

            };
            await _unitOfWork.GetRepository<Subject>().InsertAsync(newSubject);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            CreateNewSubjectResponse createNewSubjectResponse = null;
            if (isSuccessful)
            {
                createNewSubjectResponse = new CreateNewSubjectResponse()
                {
                    Id = newSubject.Id,
                    Title = newSubject.Title,
                    Description = newSubject.Description,
                    DelFlg = newSubject.DelFlg,
                    InsDate = newSubject.InsDate,
                    UpdDate = newSubject.UpdDate
                };
            }

            return createNewSubjectResponse;
        }
        public async Task<IPaginate<GetSubjectResponse>> getListSubject(int page, int size)
        {
            var subjects = await _unitOfWork.GetRepository<Subject>().GetPagingListAsync(
                selector: s => new GetSubjectResponse(s.Id, s.Title, s.Description),
                predicate: s => s.DelFlg != true,
                page: page,
                size: size
                );
            if (subjects == null)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectsIsEmpty);
            }
            return subjects;
        }
        public async Task<GetSubjectResponse> getSubjectById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
            }
            var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(
                selector: s => new GetSubjectResponse(s.Id, s.Title, s.Description),
                predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (subject == null)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
            }
            return subject;
        }
        public async Task<bool> UpdateSubject(Guid id, UpdateSubjectRequest request, Guid courseId)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
            }
            var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (subject == null)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
            }

            if (courseId != Guid.Empty)
            {
                var course = await _unitOfWork.GetRepository<Course>().SingleOrDefaultAsync(predicate: c => c.Id.Equals(courseId) && c.DelFlg != true);
                if (course == null)
                {
                    throw new BadHttpRequestException(MessageConstant.CourseMessage.CourseNotFound);
                }
                subject.CourseId = courseId;
            }


            subject.Title = string.IsNullOrEmpty(request.Title) ? subject.Title : request.Title;
            subject.Description = string.IsNullOrEmpty(request.Description) ? subject.Description : request.Description;
            subject.UpdDate = TimeUtils.GetCurrentSEATime();

            _unitOfWork.GetRepository<Subject>().UpdateAsync(subject);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }
        public async Task<bool> RemoveSubject(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
            }
            var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (subject == null)
            {

                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
            }
            subject.UpdDate = TimeUtils.GetCurrentSEATime();
            subject.DelFlg = true;
            _unitOfWork.GetRepository<Subject>().UpdateAsync(subject);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<IPaginate<GetChapterResponse>> GetListChapters(Guid id, int page, int size)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
            }
            var chapters = await _unitOfWork.GetRepository<Chapter>().GetPagingListAsync(
                selector: s => new GetChapterResponse(s.Id, s.Title, s.Description),
                predicate: s => s.SubjectId.Equals(id) && s.DelFlg != true,
                page: page,
                size: size
                );
            if (chapters == null)
            {
                throw new BadHttpRequestException(MessageConstant.ChapterMessage.ChaptersIsEmpty);
            }
            return chapters;
        }
        


    }
}
