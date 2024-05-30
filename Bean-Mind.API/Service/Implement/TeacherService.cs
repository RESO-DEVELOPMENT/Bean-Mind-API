using AutoMapper;
using Azure.Core;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload;
using Bean_Mind.API.Payload.Request;
using Bean_Mind.API.Payload.Request.Teachers;

using Bean_Mind.API.Payload.Response.Teachers;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Enums;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
namespace Bean_Mind.API.Service.Implement
{
    public class TeacherService : BaseService<TeacherService>, ITeacherService
    {
        public TeacherService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<TeacherService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }


       
        public async Task<CreateNewTeacherResponse> CreateTeacher(CreateNewTeacherResquest newTeacherRequest, Guid schoolId)
        {
            _logger.LogInformation($"Create new teacher with {newTeacherRequest.FirstName} {newTeacherRequest.LastName}");

            School school = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(schoolId) && s.DelFlg != true);
            if (school == null)
            {
                throw new BadHttpRequestException(MessageConstant.SchoolMessage.SchoolNotFound);
            }

            var accountS = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: account => account.UserName.Equals(newTeacherRequest.UserName) && account.DelFlg != true
                );
            if (accountS != null)
            {
                throw new BadHttpRequestException(MessageConstant.AccountMessage.UsernameExisted);
            }
            Account account = new Account() 
            { 
                Id = Guid.NewGuid(),
                UserName = newTeacherRequest.UserName,
                Password = PasswordUtil.HashPassword(newTeacherRequest.Password),
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false,
                Role = RoleEnum.Teacher.GetDescriptionFromEnum(),
                SchoolId = schoolId
            };

            await _unitOfWork.GetRepository<Account>().InsertAsync(account);
            var successAccount = await _unitOfWork.CommitAsync() > 0;
            if (!successAccount)
            {
                throw new BadHttpRequestException(MessageConstant.AccountMessage.CreateTeacherAccountFailMessage);
            }

            Teacher newTeacher = new Teacher()
            {
                Id = Guid.NewGuid(),
                FirstName = newTeacherRequest.FirstName,
                LastName = newTeacherRequest.LastName,
                DateOfBirth = newTeacherRequest.DateOfBirth,
                ImgUrl = newTeacherRequest.ImgUrl,
                Email = newTeacherRequest.Email,
                Phone = newTeacherRequest.Phone,
                SchoolId = schoolId,
                AccountId = account.Id,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false
            };

            await _unitOfWork.GetRepository<Teacher>().InsertAsync(newTeacher);
            var successTeacher = await _unitOfWork.CommitAsync() > 0;
            if (!successTeacher)
            {
                throw new BadHttpRequestException(MessageConstant.AccountMessage.CreateTeacherAccountFailMessage);
            }

            return new CreateNewTeacherResponse
            {
                Id = newTeacher.Id,
                FirstName = newTeacher.FirstName,
                LastName = newTeacher.LastName,
                Email = newTeacher.Email,
                Phone = newTeacher.Phone,   
                DateOfBirth = newTeacher.DateOfBirth,
                ImgUrl = newTeacher.ImgUrl,
                School = newTeacher.School,
                DelFlg = false,
                InsDate = newTeacher.InsDate,
                UpdDate= newTeacher.UpdDate
            };
        }

        public async Task<IPaginate<GetTeacherResponse>> GetAllTeachers(int page, int size )
        {
            var teachers = await _unitOfWork.GetRepository<Teacher>().GetPagingListAsync(
            selector: x => new GetTeacherResponse()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Phone = x.Phone,
                DateOfBirth = x.DateOfBirth,
                ImgUrl = x.ImgUrl,
            },
            predicate: x => x.DelFlg == false,
            size: size,
            page: page);
            return teachers;
        }

        public async Task<GetTeacherResponse> GetTeacherById(Guid teacherId)
        {

            GetTeacherResponse teachers = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
           selector: x => new GetTeacherResponse()
           {
               Id = x.Id,
               FirstName = x.FirstName,
               LastName = x.LastName,
               Email = x.Email,
               Phone = x.Phone,
               DateOfBirth = x.DateOfBirth,
               ImgUrl = x.ImgUrl,
           },
           predicate: x => x.Id.Equals(teacherId) && x.DelFlg != true
           );
           
            return teachers;

        }

        public async Task<bool> UpdateTeacher(Guid id, UpdateTecherRequest request)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.TeacherMessage.TeacherNotFound);

            Teacher teacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id) && x.DelFlg != true
            );

            if (teacher == null) throw new BadHttpRequestException(MessageConstant.TeacherMessage.TeacherNotFound);

            _logger.LogInformation($"Start to update Teacher {teacher.Id}");

            // Trim and update fields
            request.TrimString();

            teacher.FirstName = string.IsNullOrEmpty(request.FirstName) ? teacher.FirstName : request.FirstName;
            teacher.LastName = string.IsNullOrEmpty(request.LastName) ? teacher.LastName : request.LastName;
            teacher.DateOfBirth = string.IsNullOrEmpty(request.DateOfBirth.ToString()) ? teacher.DateOfBirth : request.DateOfBirth;
            teacher.Email = string.IsNullOrEmpty(request.Email) ? teacher.Email : request.Email;
            teacher.Phone = string.IsNullOrEmpty(request.Phone) ? teacher.Phone : request.Phone;
            teacher.UpdDate = TimeUtils.GetCurrentSEATime();

            _unitOfWork.GetRepository<Teacher>().UpdateAsync(teacher);
            var isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }


        public async Task<bool> RemoveTeacher(Guid teacherId)
        {

            Teacher teachers = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(teacherId) && x.DelFlg != true);
            teachers.UpdDate = TimeUtils.GetCurrentSEATime();
            teachers.DelFlg = true;
            _unitOfWork.GetRepository<Teacher>().UpdateAsync(teachers);
            var isSuccessful = await _unitOfWork.CommitAsync() > 0;


            return isSuccessful;



        }
    }
}
