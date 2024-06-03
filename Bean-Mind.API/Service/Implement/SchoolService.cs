using AutoMapper;
using Azure.Core;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Schools;
using Bean_Mind.API.Payload.Response.Chapters;
using Bean_Mind.API.Payload.Response.Curriculums;
using Bean_Mind.API.Payload.Response.Schools;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Enums;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Implement
{
    public class SchoolService : BaseService<SchoolService>, ISchoolService
    {
        public SchoolService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<SchoolService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateNewSchoolResponse> CreateNewSchool(CreateNewSchoolRequest createNewSchoolRequest)
        {
            _logger.LogInformation($"Create new School with {createNewSchoolRequest.Name}");
            School newSchool = new School()
            {
                Id = Guid.NewGuid(),
                Name = createNewSchoolRequest.Name,
                Address = createNewSchoolRequest.Address,
                Phone = createNewSchoolRequest.Phone,
                Logo = createNewSchoolRequest.Logo,
                Description = createNewSchoolRequest.Description,
                Email = createNewSchoolRequest.Email,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false,
            };
            
            await _unitOfWork.GetRepository<School>().InsertAsync(newSchool);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            Account account = new Account()
            {
                Id = Guid.NewGuid(),
                UserName = createNewSchoolRequest.UserName,
                Password = PasswordUtil.HashPassword(createNewSchoolRequest.Password),
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false,
                Role = RoleEnum.SysSchool.GetDescriptionFromEnum(),
                SchoolId = newSchool.Id,
            };
            await _unitOfWork.GetRepository<Account>().InsertAsync(account);
            await _unitOfWork.CommitAsync();
            CreateNewSchoolResponse createNewSchoolResponse = null;
            if (isSuccessful)
            {
                createNewSchoolResponse = new CreateNewSchoolResponse()
                {
                    Name = newSchool.Name,
                    Address = newSchool.Address,
                    Phone = newSchool.Phone,
                    Logo = newSchool.Logo,
                    Description = newSchool.Description,
                    Email = newSchool.Email,
                };
            }

            return createNewSchoolResponse;
        }

        public async Task<bool> deleteSchool(Guid Id)
        {
            if(Id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.SchoolMessage.SchoolNotFound);
            }
            var school = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(Id) && s.DelFlg != true);
            if(school == null)
            {
                throw new BadHttpRequestException(MessageConstant.SchoolMessage.SchoolNotFound);
            }
            
            var parents = await _unitOfWork.GetRepository<Parent>().GetListAsync(
                predicate: p => p.SchoolId.Equals(Id) && p.DelFlg == false);
            foreach(var parent in parents)
            {
                var students = await _unitOfWork.GetRepository<Student>().GetListAsync(
                    predicate: s => s.ParentId.Equals(parent.Id)  && s.DelFlg == false);
                foreach(var student in students)
                {
                    var accountS = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                        predicate: a => a.Id.Equals(student.AccountId) && a.DelFlg == false);
                    accountS.DelFlg = true;
                    accountS.UpdDate = TimeUtils.GetCurrentSEATime();
                    _unitOfWork.GetRepository<Account>().UpdateAsync(accountS);
                    student.DelFlg = true;
                    student.UpdDate = TimeUtils.GetCurrentSEATime();
                    _unitOfWork.GetRepository<Student>().UpdateAsync(student);
                }
                var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                        predicate: a => a.Id.Equals(parent.AccountId) && a.DelFlg == false);
                account.DelFlg = true;
                account.UpdDate = TimeUtils.GetCurrentSEATime();
                _unitOfWork.GetRepository<Account>().UpdateAsync(account);
                parent.DelFlg = true;
                parent.UpdDate = TimeUtils.GetCurrentSEATime();
                _unitOfWork.GetRepository<Parent>().UpdateAsync(parent);
            }

            var currilums = await _unitOfWork.GetRepository<Curriculum>().GetListAsync(
                predicate: c => c.SchoolId.Equals(Id) && c.DelFlg == false);
            foreach (var currilum in currilums)
            {
                var courses = await _unitOfWork.GetRepository<Course>().GetListAsync(
                    predicate: c => c.CurriculumId.Equals(currilum.Id) && c.DelFlg == false);
                foreach(var course in courses)
                {
                    var subjects = await _unitOfWork.GetRepository<Subject>().GetListAsync(
                        predicate: s => s.CourseId.Equals(course.Id) && s.DelFlg == false);
                    foreach(var subject in subjects)
                    {
                        var chapters = await _unitOfWork.GetRepository<Chapter>().GetListAsync(
                            predicate: c => c.SubjectId.Equals(subject.Id) && c.DelFlg == false);
                        foreach(var chapter in chapters)
                        {
                            var topics = await _unitOfWork.GetRepository<Topic>().GetListAsync(
                                predicate: t => t.ChapterId.Equals(chapter.Id) && t.DelFlg == false);
                            foreach(var topic in topics)
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
                currilum.DelFlg = true;
                currilum.UpdDate= TimeUtils.GetCurrentSEATime();
                _unitOfWork.GetRepository<Curriculum>().UpdateAsync(currilum);
            }
            school.UpdDate = TimeUtils.GetCurrentSEATime();
            school.DelFlg = true;
            _unitOfWork.GetRepository<School>().UpdateAsync(school);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<IPaginate<GetCurriculumResponse>> GetListCurriculum(Guid id, int page, int size)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.SchoolMessage.SchoolNotFound);
            }
            var curriculums = await _unitOfWork.GetRepository<Curriculum>().GetPagingListAsync(
                selector: s => new GetCurriculumResponse
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    SchoolId = s.SchoolId,
                    InsDate = s.InsDate,
                    UpdDate = s.UpdDate,
                    DelFlg = s.DelFlg,
                },
                predicate: s => s.SchoolId.Equals(id) && s.DelFlg != true,
                page: page,
                size: size
                );
            if (curriculums == null)
            {
                throw new BadHttpRequestException(MessageConstant.CurriculumMessage.CurriculumsIsEmpty);
            }
            return curriculums;
        }

        public async Task<IPaginate<GetSchoolResponse>> getListSchool(int page, int size)
        {
            var schools = await _unitOfWork.GetRepository<School>().GetPagingListAsync(
                selector: s => new GetSchoolResponse(s.Id, s.Name, s.Address, s.Phone, s.Email, s.Logo, s.Description),
                predicate: s => s.DelFlg != true,
                page: page, size: size,
                orderBy: s => s.OrderBy(s => s.InsDate));
            return schools;

        }


        public async Task<GetSchoolResponse> getSchoolById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.SchoolMessage.SchoolNotFound);
            }
            var school = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync(
                selector: s => new GetSchoolResponse(s.Id, s.Name, s.Address, s.Phone, s.Email, s.Logo, s.Description),
                predicate: s => s.Id.Equals(id) && s.DelFlg != true
        );
            if (school == null)
            {
                throw new BadHttpRequestException(MessageConstant.SchoolMessage.SchoolNotFound);
            }
            return school;
        }

        public async Task<bool> updateSchool(UpdateSchoolRequest request, Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.SchoolMessage.SchoolNotFound);
            }
            var school = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(Id) && s.DelFlg != true);
            if (school == null)
            {
                throw new BadHttpRequestException(MessageConstant.SchoolMessage.SchoolNotFound);
            }
            school.Name = string.IsNullOrEmpty(request.Name) ? school.Name : request.Name;
            school.Address = string.IsNullOrEmpty(request.Address) ? school.Address : request.Address;
            school.Phone = string.IsNullOrEmpty(request.Phone) ? school.Phone : request.Phone;
            school.Logo = string.IsNullOrEmpty(request.Logo) ? school.Logo : request.Logo;
            school.Description = string.IsNullOrEmpty(request.Description) ? school.Description : request.Description;
            school.Email = string.IsNullOrEmpty(request.Email) ? school.Email : request.Email;
            school.UpdDate = TimeUtils.GetCurrentSEATime();

            _unitOfWork.GetRepository<School>().UpdateAsync(school);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
