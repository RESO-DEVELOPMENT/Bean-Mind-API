using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Courses;
using Bean_Mind.API.Payload.Request.Subjects;
using Bean_Mind.API.Payload.Response.Chapters;
using Bean_Mind.API.Payload.Response.Courses;
using Bean_Mind.API.Payload.Response.Subjects;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;

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
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var subjectTitle = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(
                predicate: s => s.Title.Equals(request.Title) && s.DelFlg != true && s.SchoolId.Equals(account.SchoolId));
            if (subjectTitle != null)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectTitleExisted);
            }

            var subjectCode = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(
                predicate: s => s.SubjectCode.Equals(request.SubjectCode) && s.DelFlg != true && s.SchoolId.Equals(account.SchoolId));
            if (subjectCode != null)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectCodeExisted);
            }

            var  newSubject = new Subject
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                SubjectCode = request.SubjectCode,
                Description = request.Description,
                SchoolId = account.SchoolId.Value,
                DelFlg = false,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                

            };
            if (courseId != Guid.Empty)
            {

                var  course = await _unitOfWork.GetRepository<Course>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(courseId) && s.DelFlg != true);
                if (course == null)
                {
                    _logger.LogError($"CourseId with id {courseId} not found.");
                    return null;
                }
                newSubject.CourseId = courseId ;
            }

            
            await _unitOfWork.GetRepository<Subject>().InsertAsync(newSubject);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            CreateNewSubjectResponse createNewSubjectResponse = null;
            if (isSuccessful)
            {
                createNewSubjectResponse = new CreateNewSubjectResponse()
                {
                    Id = newSubject.Id,
                    Title = newSubject.Title,
                    SubjectCode = newSubject.SubjectCode,
                    Description = newSubject.Description,
                    CourseId = newSubject.CourseId,
                    SchoolId = account.SchoolId,
                    DelFlg = newSubject.DelFlg,
                    InsDate = newSubject.InsDate,
                    UpdDate = newSubject.UpdDate
                };
            }

            return createNewSubjectResponse;
        }
        public async Task<IPaginate<GetSubjectResponse>> getListSubject(int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var subjects = await _unitOfWork.GetRepository<Subject>().GetPagingListAsync(
                selector: s => new GetSubjectResponse(s.Id, s.Title, s.SubjectCode,s.Description, s.CourseId, s.SchoolId),
                predicate: s => s.DelFlg != true && s.SchoolId.Equals(account.SchoolId),
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
                selector: s => new GetSubjectResponse(s.Id, s.Title, s.SubjectCode,s.Description, s.CourseId, s.SchoolId),
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
            subject.SubjectCode = string.IsNullOrEmpty(request.SubjectCode) ? subject.SubjectCode : request.SubjectCode;

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

            var chapters = await _unitOfWork.GetRepository<Chapter>().GetListAsync(predicate: s => s.SubjectId.Equals(id) && s.DelFlg != true);

            foreach (var chapter in chapters)
            {
                var topics = await _unitOfWork.GetRepository<Topic>().GetListAsync(predicate: s => s.ChapterId.Equals(chapter.Id) && s.DelFlg != true);
                foreach (var topic in topics)
                {
                    foreach (var activity in topic.Activities)
                    {
                        foreach (var video in activity.Videos)
                        {
                            video.DelFlg = true;
                            video.UpdDate = TimeUtils.GetCurrentSEATime();
                            _unitOfWork.GetRepository<Video>().UpdateAsync(video);
                        }
                        foreach (var document in activity.Documents)
                        {
                            document.DelFlg = true;
                            document.UpdDate = TimeUtils.GetCurrentSEATime();
                            _unitOfWork.GetRepository<Document>().UpdateAsync(document);
                        }
                        foreach (var workSheet in activity.WorkSheets)
                        {
                            foreach (var worksheetQuestion in workSheet.WorksheetQuestions)
                            {
                                worksheetQuestion.DelFlg = true;
                                worksheetQuestion.UpdDate = TimeUtils.GetCurrentSEATime();
                                _unitOfWork.GetRepository<WorksheetQuestion>().UpdateAsync(worksheetQuestion);
                            }
                            workSheet.DelFlg = true;
                            workSheet.UpdDate = TimeUtils.GetCurrentSEATime();
                            _unitOfWork.GetRepository<WorkSheet>().UpdateAsync(workSheet);
                        }
                        activity.DelFlg = true;
                        activity.UpdDate = TimeUtils.GetCurrentSEATime();
                        _unitOfWork.GetRepository<Activity>().UpdateAsync(activity);
                    }
                    topic.UpdDate = TimeUtils.GetCurrentSEATime();
                    topic.DelFlg = true;
                    _unitOfWork.GetRepository<Topic>().UpdateAsync(topic);
                }
                chapter.UpdDate = TimeUtils.GetCurrentSEATime();
                chapter.DelFlg = true;
                _unitOfWork.GetRepository<Chapter>().UpdateAsync(chapter);
            }
            var teacherTechables = await _unitOfWork.GetRepository<TeacherTeachable>().GetListAsync(
                predicate: t => t.SubjectId.Equals(subject.Id) && t.DelFlg != true);
            foreach (var teacherTechable in teacherTechables)
            {
                teacherTechable.DelFlg = true;
                teacherTechable.UpdDate = TimeUtils.GetCurrentSEATime();
                _unitOfWork.GetRepository<TeacherTeachable>().UpdateAsync(teacherTechable);
            }

            var worksheetTemplates = await _unitOfWork.GetRepository<WorksheetTemplate>().GetListAsync(
                predicate: w => w.SubjectId.Equals(subject.Id) && w.DelFlg != true);
            foreach (var worksheetTemplate in worksheetTemplates)
            {
                worksheetTemplate.DelFlg = true;
                worksheetTemplate.UpdDate = TimeUtils.GetCurrentSEATime();
                _unitOfWork.GetRepository<WorksheetTemplate>().UpdateAsync(worksheetTemplate);
            }

            _unitOfWork.GetRepository<Subject>().UpdateAsync(subject);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<IPaginate<GetChapterResponse>> GetListChapter(Guid id, int page, int size)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
            }
            var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (subject == null)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectNotFound);
            }
            var chapters = await _unitOfWork.GetRepository<Chapter>().GetPagingListAsync(
                selector: s => new GetChapterResponse(s.Id, s.Title, s.Description, s.SubjectId, s.SchoolId),
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

        public async Task<IPaginate<GetSubjectResponse>> GetListSubjectByTitle(string title, int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");
            var subjects = await _unitOfWork.GetRepository<Subject>().GetPagingListAsync(
                selector: s => new GetSubjectResponse(s.Id, s.Title, s.SubjectCode, s.Description, s.CourseId, s.SchoolId),
                predicate: s => s.Title.Contains(title) && s.DelFlg == false && s.SchoolId.Equals(account.SchoolId),
                page: page,
                size: size
                );
            if (subjects == null)
            {
                throw new BadHttpRequestException(MessageConstant.SubjectMessage.SubjectsIsEmpty);
            }
            return subjects;
        }

        public async Task<IPaginate<GetSubjectResponse>> GetListSubjectByCode(string code, int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");
            var subjects = await _unitOfWork.GetRepository<Subject>().GetPagingListAsync(
                selector: s => new GetSubjectResponse(s.Id, s.Title, s.SubjectCode, s.Description, s.CourseId, s.SchoolId),
                predicate: s => s.SubjectCode.Contains(code) && s.DelFlg == false && s.SchoolId.Equals(account.SchoolId),
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
