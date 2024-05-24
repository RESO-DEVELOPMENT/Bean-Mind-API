using AutoMapper;
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

            Account account = new Account() 
            { 
                Id = Guid.NewGuid(),
                UserName = newTeacherRequest.UserName,
                Password = PasswordUtil.HashPassword(newTeacherRequest.Password),
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false,
                Role = RoleEnum.Teacher.GetDescriptionFromEnum(),
                SchoolId = schoolId
            };

            await _unitOfWork.GetRepository<Account>().InsertAsync(account);
            var successAccount = await _unitOfWork.CommitAsync() > 0;
            if (!successAccount)
            {
                return null; // Hoặc trả về phản hồi lỗi phù hợp
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
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false
            };

            await _unitOfWork.GetRepository<Teacher>().InsertAsync(newTeacher);

            var successTeacher = await _unitOfWork.CommitAsync() > 0;
            if (!successTeacher)
            {
                return null; // Hoặc trả về phản hồi lỗi phù hợp
            }

            return new CreateNewTeacherResponse
            {
                Id = newTeacher.Id,
                FirstName = newTeacher.FirstName,
                LastName = newTeacher.LastName,
                Email = newTeacher.Email,
                Phone = newTeacher.Phone,
                Message = "Teacher created successfully"
            };
        }
        public async Task<IPaginate<TeacherResponse>> GetAllTeachers(int page, int size )
        {
            var teachers = await _unitOfWork.GetRepository<Teacher>().GetPagingListAsync(
            selector: x => new TeacherResponse()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Phone = x.Phone,
                DateOfBirth = x.DateOfBirth,
                ImgUrl = x.ImgUrl,
                SchoolId = x.SchoolId
            },
            predicate: x => x.DelFlg == false,
            size: size,
            page: page);
            return teachers;
        }

        public async Task<TeacherResponse> GetTeacherById(Guid teacherId)
        {

            TeacherResponse teachers = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
           selector: x => new TeacherResponse()
           {
               Id = x.Id,
               FirstName = x.FirstName,
               LastName = x.LastName,
               Email = x.Email,
               Phone = x.Phone,
               DateOfBirth = x.DateOfBirth,
               ImgUrl = x.ImgUrl,
               SchoolId = x.SchoolId
           },
           predicate: x => x.Id.Equals(teacherId)
           );
           
            return teachers;

        }

        public async Task<bool> UpdateTeacher(Guid id, UpdateTecherRequest request)
        {
            if (id == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Teacher.EmptyCategoryIdMessage);

            Teacher teacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id)
            );

            if (teacher == null) throw new BadHttpRequestException(MessageConstant.Teacher.TeacherNotFound);

            _logger.LogInformation($"Start to update Teacher {teacher.Id}");

            // Trim and update fields
            request.TrimString();

            teacher.FirstName = string.IsNullOrEmpty(request.FirstName) ? teacher.FirstName : request.FirstName;
            teacher.LastName = string.IsNullOrEmpty(request.LastName) ? teacher.LastName : request.LastName;
            teacher.DateOfBirth = request.DateOfBirth ?? teacher.DateOfBirth;
            teacher.Email = string.IsNullOrEmpty(request.Email) ? teacher.Email : request.Email;
            teacher.Phone = string.IsNullOrEmpty(request.Phone) ? teacher.Phone : request.Phone;
            teacher.UpdDate = DateTime.UtcNow;

            _unitOfWork.GetRepository<Teacher>().UpdateAsync(teacher);
            var isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }


        public async Task<bool> RemoveTeacher(Guid teacherId)
        {

            Teacher teachers = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(teacherId));
            teachers.DelFlg = true;
            _unitOfWork.GetRepository<Teacher>().UpdateAsync(teachers);
            var isSuccessful = await _unitOfWork.CommitAsync() > 0;


            return isSuccessful;



        }
    }
}
