using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Schools;
using Bean_Mind.API.Payload.Request.Teachers;
using Bean_Mind.API.Payload.Response.Teachers;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Enums;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace Bean_Mind.API.Service.Implement
{
    public class TeacherService : BaseService<TeacherService>, ITeacherService
    {
        public TeacherService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<TeacherService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }


       
        public async Task<CreateNewTeacherResponse> CreateTeacher(CreateNewTeacherResquest newTeacherRequest)
        {
            _logger.LogInformation($"Create new teacher with {newTeacherRequest.FirstName} {newTeacherRequest.LastName}");

            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var accountExist = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (accountExist == null)
                throw new Exception("Account or SchoolId is null");

            string phonePattern = @"^0\d{9}$";
            if (!Regex.IsMatch(newTeacherRequest.Phone, phonePattern))
            {
                throw new BadHttpRequestException(MessageConstant.PatternMessage.PhoneIncorrect);
            }

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(newTeacherRequest.Email, emailPattern))
            {
                throw new BadHttpRequestException(MessageConstant.PatternMessage.EmailIncorrect);
            }

            Teacher phoneTeacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: t => t.Phone.Equals(newTeacherRequest.Phone) && t.DelFlg != true);
            if(phoneTeacher != null)
            {
                throw new BadHttpRequestException(MessageConstant.TeacherMessage.TeacherPhoneExisted);
            }

            Teacher emailTeacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: t => t.Email.Equals(newTeacherRequest.Email) && t.DelFlg != true);
            if (emailTeacher != null)
            {
                throw new BadHttpRequestException(MessageConstant.TeacherMessage.TeacherEmailExisted);
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
                SchoolId = accountExist.SchoolId
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
                DelFlg = false,
                InsDate = newTeacher.InsDate,
                UpdDate= newTeacher.UpdDate
            };
        }

        public async Task<IPaginate<GetTeacherResponse>> GetAllTeachers(int page, int size )
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

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
            include: s => s.Include(s => s.Account),
            predicate: x => x.DelFlg == false && x.Account.SchoolId.Equals(account.SchoolId),
            size: size,
            page: page);
            return teachers;
        }

        public async Task<GetTeacherResponse> GetTeacherById(Guid id)
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
           predicate: x => x.Id.Equals(id) && x.DelFlg != true
           );
           
            return teachers;

        }

        public async Task<bool> RemoveTeacher(Guid id)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.TeacherMessage.TeacherNotFound);

            Teacher teachers = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(id) && x.DelFlg != true);
            if (teachers == null) throw new BadHttpRequestException(MessageConstant.TeacherMessage.TeacherNotFound);

            teachers.UpdDate = TimeUtils.GetCurrentSEATime();
            teachers.DelFlg = true;
            
            var teacherTeachables = await _unitOfWork.GetRepository<TeacherTeachable>().GetListAsync(
                predicate: tt => tt.TeacherId.Equals(id) && tt.DelFlg != true
            );
            foreach (var teacherTeachable in teacherTeachables)
            {
                teacherTeachable.DelFlg = true;
                teacherTeachable.UpdDate = TimeUtils.GetCurrentSEATime();
                _unitOfWork.GetRepository<TeacherTeachable>().UpdateAsync(teacherTeachable);
            }
            var account = await _unitOfWork.GetRepository<Account>()
                                            .SingleOrDefaultAsync(predicate: a => a.Id.Equals(teachers.AccountId) && a.DelFlg != true);
            if (account != null)
            {
                account.DelFlg = true;
                account.UpdDate = TimeUtils.GetCurrentSEATime();
                _unitOfWork.GetRepository<Account>().UpdateAsync(account);
            }
            
            _unitOfWork.GetRepository<Teacher>().UpdateAsync(teachers);
            var isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
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
            teacher.DateOfBirth = request.DateOfBirth ?? teacher.DateOfBirth;
            teacher.Email = string.IsNullOrEmpty(request.Email) ? teacher.Email : request.Email;
            teacher.Phone = string.IsNullOrEmpty(request.Phone) ? teacher.Phone : request.Phone;
            teacher.UpdDate = TimeUtils.GetCurrentSEATime();

            _unitOfWork.GetRepository<Teacher>().UpdateAsync(teacher);
            var isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }
    }
}
