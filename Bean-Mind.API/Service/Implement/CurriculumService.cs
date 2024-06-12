using AutoMapper;
using AutoMapper;
using Azure.Core;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Curriculums;
using Bean_Mind.API.Payload.Response.Chapters;
using Bean_Mind.API.Payload.Response.Courses;
using Bean_Mind.API.Payload.Response.Curriculums;
using Bean_Mind.API.Payload.Response.Schools;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensibility;
using System.Data;
using System.Drawing;
using System.Security.Claims;

namespace Bean_Mind.API.Service.Implement
{
    public class CurriculumService : BaseService<CurriculumService>, ICurriculumService
    {
        public CurriculumService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<CurriculumService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateNewCurriculumResponse> CreateNewCurriculum(CreateNewCurriculumRequest createNewCurriculumRequest)
        {
            _logger.LogInformation($"Create new Curriculum with {createNewCurriculumRequest.Title}");
            //Get AccountId of User curent from httpcontext 
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");
            Curriculum newCurriculum = new Curriculum()
            {
                Id = Guid.NewGuid(),
                Title = createNewCurriculumRequest.Title,
                Description = createNewCurriculumRequest.Description,
                StartDate = createNewCurriculumRequest.StartDate,
                EndDate = createNewCurriculumRequest.EndDate,
                SchoolId = account.SchoolId.Value,//Account Id for School in these Case not null and .value same as get real value for it  
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false

            };



            await _unitOfWork.GetRepository<Curriculum>().InsertAsync(newCurriculum);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            CreateNewCurriculumResponse createNewCurriculumResponse = null;
            if (isSuccessful)
            {
                createNewCurriculumResponse = new CreateNewCurriculumResponse()
                {
                    Id = newCurriculum.Id,
                    Title = newCurriculum.Title,
                    Description = newCurriculum.Description,
                    StartDate = newCurriculum.StartDate,
                    EndDate = newCurriculum.EndDate,
                    SchoolId = newCurriculum.SchoolId.Value,
                    InsDate = newCurriculum.InsDate,
                    UpdDate = newCurriculum.UpdDate,
                    DelFlg = newCurriculum.DelFlg


                };
            }

            return createNewCurriculumResponse;
        }

        public async Task<bool> deleteCurriculum(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.CurriculumMessage.CurriculumNotFound);
            }
            var curriculum = await _unitOfWork.GetRepository<Curriculum>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(Id) && s.DelFlg != true);
            if (curriculum == null)
            {

                throw new BadHttpRequestException(MessageConstant.CurriculumMessage
                    .CurriculumNotFound);
            }

            var courses = await _unitOfWork.GetRepository<Course>().GetListAsync(predicate: c => c.CurriculumId.Equals(Id) && c.DelFlg != true);
            foreach (var course in courses)
            {
                var subjects = await _unitOfWork.GetRepository<Subject>().GetListAsync(
                        predicate: s => s.CourseId.Equals(course.Id) && s.DelFlg == false);
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
            }
            curriculum.DelFlg = true;
            curriculum.UpdDate = TimeUtils.GetCurrentSEATime();
            _unitOfWork.GetRepository<Curriculum>().UpdateAsync(curriculum);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<IPaginate<GetCourseResponse>> GetListCourses(Guid id, int page, int size)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.CurriculumMessage.CurriculumNotFound);
            }
            var curriculums = await _unitOfWork.GetRepository<Curriculum>().SingleOrDefaultAsync(
                 predicate: x => x.Id == id && x.DelFlg != true
                 );
            if (curriculums == null)
            {
                throw new BadHttpRequestException(MessageConstant.CurriculumMessage.CurriculumNotFound);
            }
            var courses = await _unitOfWork.GetRepository<Course>().GetPagingListAsync(
                selector: s => new GetCourseResponse
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    Status = s.Status,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    CurriculumId = s.CurriculumId
                },
                predicate: s => s.CurriculumId.Equals(id) && s.DelFlg != true,
                page: page,
                size: size
                );
            if (courses == null)
            {
                throw new BadHttpRequestException(MessageConstant.CourseMessage.CoursesIsEmpty);
            }
            return courses;
        }


            public async Task<GetCurriculumResponse> getCurriculumById(Guid Id)
            {
                var curriculums = await _unitOfWork.GetRepository<Curriculum>().SingleOrDefaultAsync(
                 selector: s => new GetCurriculumResponse
                 {
                     Id = s.Id,
                     Title = s.Title,
                     Description = s.Description,
                     StartDate = s.StartDate,
                     EndDate = s.EndDate,
                     SchoolId = s.SchoolId
                 },
                 predicate: x => x.Id == Id && x.DelFlg != true
                 );

                return curriculums;
            }

            public async Task<IPaginate<GetCurriculumResponse>> getListCurriculum(int page, int size)
            {
                var curriculums = await _unitOfWork.GetRepository<Curriculum>().GetPagingListAsync(
                  selector: s => new GetCurriculumResponse
                  {
                      Id = s.Id,
                      Title = s.Title,
                      Description = s.Description,
                      StartDate = s.StartDate,
                      EndDate = s.EndDate,
                      SchoolId = s.SchoolId
                  },
                  predicate: x => x.DelFlg == false,
                  size: size,
                  page: page);

                return curriculums;
            }

            public async Task<bool> updateCurriculum(Guid Id, UpdateCurriculumRequest updateCurriculumRequest, Guid SchoolId)
            {
                if (Id == Guid.Empty)
                {
                    throw new BadHttpRequestException(MessageConstant.CurriculumMessage.CurriculumNotFound);
                }
                var curriculum = await _unitOfWork.GetRepository<Curriculum>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(Id) && s.DelFlg != true);
                if (curriculum == null)
                {
                    throw new BadHttpRequestException(MessageConstant.CurriculumMessage.CurriculumNotFound);
                }
                if (SchoolId != Guid.Empty)
                {
                    School school = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(SchoolId) && s.DelFlg != true);
                    if (school == null)
                    {
                        throw new BadHttpRequestException(MessageConstant.SchoolMessage.SchoolNotFound);
                    }
                    curriculum.SchoolId = SchoolId;
                }

                curriculum.Title = string.IsNullOrEmpty(updateCurriculumRequest.Title) ? curriculum.Title : updateCurriculumRequest.Title;
                curriculum.Description = string.IsNullOrEmpty(updateCurriculumRequest.Description) ? curriculum.Description : updateCurriculumRequest.Description;


                curriculum.StartDate = updateCurriculumRequest.StartDate ?? curriculum.StartDate;
                curriculum.EndDate = updateCurriculumRequest.EndDate ?? curriculum.EndDate;
                curriculum.UpdDate = TimeUtils.GetCurrentSEATime();

                _unitOfWork.GetRepository<Curriculum>().UpdateAsync(curriculum);
                bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
                return isSuccessful;
            }
        }
    }

