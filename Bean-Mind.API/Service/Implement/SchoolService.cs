﻿using AutoMapper;
using Azure.Core;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Schools;
using Bean_Mind.API.Payload.Response.Curriculums;
using Bean_Mind.API.Payload.Response.QuestionLevels;
using Bean_Mind.API.Payload.Response.Schools;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Enums;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using Google.Apis.Drive.v3;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Bean_Mind.API.Service.Implement
{
    public class SchoolService : BaseService<SchoolService>, ISchoolService
    {
        private readonly GoogleDriveService _driveService;

        public SchoolService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<SchoolService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, GoogleDriveService driveService) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
            _driveService = driveService;
        }

        public async Task<CreateNewSchoolResponse> CreateNewSchool(CreateNewSchoolRequest createNewSchoolRequest)
        {
            _logger.LogInformation($"Create new School with {createNewSchoolRequest.Name}");

            // Kiểm tra định dạng số điện thoại
            string phonePattern = @"^0\d{9}$";
            if (!Regex.IsMatch(createNewSchoolRequest.Phone, phonePattern))
            {
                throw new BadHttpRequestException(MessageConstant.PatternMessage.PhoneIncorrect);
            }

            // Kiểm tra định dạng email
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(createNewSchoolRequest.Email, emailPattern))
            {
                throw new BadHttpRequestException(MessageConstant.PatternMessage.EmailIncorrect);
            }

            // Kiểm tra số điện thoại đã tồn tại
            School phoneSchool = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync(
                predicate: s => s.Phone.Equals(createNewSchoolRequest.Phone) && s.DelFlg != true);
            if (phoneSchool != null)
            {
                throw new BadHttpRequestException(MessageConstant.SchoolMessage.SchoolPhoneExisted);
            }

            // Kiểm tra email đã tồn tại
            School emailSchool = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync(
                predicate: s => s.Email.Equals(createNewSchoolRequest.Email) && s.DelFlg != true);
            if (emailSchool != null)
            {
                throw new BadHttpRequestException(MessageConstant.SchoolMessage.SchoolEmailExisted);
            }

            // Tải lên Google Drive và lấy URL
            string url   = await _driveService.UploadToGoogleDriveAsync(createNewSchoolRequest.Logo);

            // Tạo đối tượng School mới
            School newSchool = new School()
            {
                Id = Guid.NewGuid(),
                Name = createNewSchoolRequest.Name,
                Address = createNewSchoolRequest.Address,
                Phone = createNewSchoolRequest.Phone,
                Logo = url,
                Description = createNewSchoolRequest.Description,
                Email = createNewSchoolRequest.Email,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false,
            };

            await _unitOfWork.GetRepository<School>().InsertAsync(newSchool);
            bool isSchoolCreated = await _unitOfWork.CommitAsync() > 0;

            // Kiểm tra tên tài khoản đã tồn tại
            var accountS = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: account => account.UserName.Equals(createNewSchoolRequest.UserName) && account.DelFlg != true);
            if (accountS != null)
            {
                throw new BadHttpRequestException(MessageConstant.AccountMessage.UsernameExisted);
            }

            // Tạo đối tượng Account mới
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

            if (isSchoolCreated)
            {
                createNewSchoolResponse = new CreateNewSchoolResponse()
                {
                    Name = newSchool.Name,
                    Address = newSchool.Address,
                    Phone = newSchool.Phone,
                    Logo = newSchool.Logo,
                    Description = newSchool.Description,
                    Email = newSchool.Email,
                    InsDate = newSchool.InsDate,
                    UpdDate = newSchool.UpdDate,
                    DelFlg = newSchool.DelFlg
                };
            }

            return createNewSchoolResponse;
        }



        public async Task<bool> deleteSchool(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.SchoolMessage.SchoolNotFound);
            }
            var school = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(Id) && s.DelFlg != true);
            if (school == null)
            {
                throw new BadHttpRequestException(MessageConstant.SchoolMessage.SchoolNotFound);
            }

            var parents = await _unitOfWork.GetRepository<Parent>().GetListAsync(
                predicate: p => p.Account.SchoolId.Equals(Id) && p.DelFlg == false,
                include: p => p.Include(p => p.Account)
                );
            foreach (var parent in parents)
            {
                var students = await _unitOfWork.GetRepository<Student>().GetListAsync(
                    predicate: s => s.ParentId.Equals(parent.Id) && s.DelFlg == false);
                foreach (var student in students)
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
                                topic.DelFlg = true;
                                topic.UpdDate = TimeUtils.GetCurrentSEATime();
                                _unitOfWork.GetRepository<Topic>().UpdateAsync(topic);
                            }
                            chapter.DelFlg = true;
                            chapter.UpdDate = TimeUtils.GetCurrentSEATime();
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
                        subject.DelFlg = true;
                        subject.UpdDate = TimeUtils.GetCurrentSEATime();
                        _unitOfWork.GetRepository<Subject>().UpdateAsync(subject);
                    }
                    course.DelFlg = true;
                    course.UpdDate = TimeUtils.GetCurrentSEATime();
                    _unitOfWork.GetRepository<Course>().UpdateAsync(course);
                }
                currilum.DelFlg = true;
                currilum.UpdDate = TimeUtils.GetCurrentSEATime();
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
            var school = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (school == null)
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
                    SchoolId = s.SchoolId
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

        public async Task<IPaginate<GetQuestionLevelResponse>> GetListQuestionLevel(Guid id, int page, int size)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.SchoolMessage.SchoolNotFound);
            }
            var school = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (school == null)
            {
                throw new BadHttpRequestException(MessageConstant.SchoolMessage.SchoolNotFound);
            }

            var questionLevels = await _unitOfWork.GetRepository<QuestionLevel>().GetPagingListAsync(
              selector: s => new GetQuestionLevelResponse
              {
                  Id = s.Id,
                  Level = s.Level,
                  SchoolId = s.SchoolId
              },
            predicate: x => x.SchoolId.Equals(id) && x.DelFlg != true,
              size: size,
              page: page);

            return questionLevels;
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
