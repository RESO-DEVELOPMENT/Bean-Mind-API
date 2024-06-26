using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Students;
using Bean_Mind.API.Payload.Response.Students;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Enums;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using Microsoft.EntityFrameworkCore;

namespace Bean_Mind.API.Service.Implement
{
    public class StudentService : BaseService<StudentService>, IStudentService
    {
        public StudentService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<StudentService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateNewStudentResponse> CreateNewStudent(CreateNewStudentRequest request, Guid parentId)
        {
            _logger.LogInformation($"Create new Student with {request.FirstName}  {request.LastName}");

            //Check School and Parent
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var accountExist = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (accountExist == null)
                throw new Exception("Account or SchoolId is null");

            Parent parent = await _unitOfWork.GetRepository<Parent>().SingleOrDefaultAsync(predicate: p => p.Id.Equals(parentId) && p.DelFlg != true);
            if (parent == null)
            {
                throw new BadHttpRequestException(MessageConstant.ParentMessage.ParentNotFound);
            }

            //Create Account
            var accountS = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: account => account.UserName.Equals(request.UserName) && account.DelFlg != true
                );
            if (accountS != null)
            {
                throw new BadHttpRequestException(MessageConstant.AccountMessage.UsernameExisted);
            }
            Account account = new Account()
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Password = PasswordUtil.HashPassword(request.Password),
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false,
                Role = RoleEnum.Student.GetDescriptionFromEnum(),
                SchoolId = accountExist.SchoolId
            };
            await _unitOfWork.GetRepository<Account>().InsertAsync(account);
            var successAccount = await _unitOfWork.CommitAsync() > 0;
            if (!successAccount)
            {
                throw new BadHttpRequestException(MessageConstant.AccountMessage.CreateStudentAccountFailMessage);
            }

            //Create Student
            Student newStudent = new Student()
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                ImgUrl = request.ImgUrl,
                DelFlg = false,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                ParentId = parentId,
                AccountId = account.Id,
            };
            await _unitOfWork.GetRepository<Student>().InsertAsync(newStudent);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            CreateNewStudentResponse createNewStudentResponse = null;
            if (isSuccessful)
            {
                createNewStudentResponse = new CreateNewStudentResponse()
                {
                    Id = newStudent.Id,
                    FirstName = newStudent.FirstName,
                    LastName = newStudent.LastName,
                    DateOfBirth = newStudent.DateOfBirth,
                    ImgUrl = newStudent.ImgUrl,
                    DelFlg = newStudent.DelFlg,
                    InsDate = newStudent.InsDate,
                    UpdDate = newStudent.UpdDate

                };
            }

            return createNewStudentResponse;
        }

        public async Task<IPaginate<GetStudentResponse>> getListStudent(int page, int size)
        {
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            var students = await _unitOfWork.GetRepository<Student>().GetPagingListAsync(
                selector: s => new GetStudentResponse(s.Id, s.FirstName, s.LastName, s.DateOfBirth, s.ImgUrl),
                include: s => s.Include(s => s.Account),
                predicate: s => s.DelFlg != true && s.Account.SchoolId.Equals(account.SchoolId),
                page: page,
                size: size
                );
            return students;
        }

        public async Task<GetStudentResponse> getStudentById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.StudentMessage.StudentNotFound);
            }
            var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(
                selector: s => new GetStudentResponse(s.Id, s.FirstName, s.LastName, s.DateOfBirth, s.ImgUrl),
                predicate: s => s.Id.Equals(id) && s.DelFlg != true
                );
            if (student == null)
            {
                throw new BadHttpRequestException(MessageConstant.StudentMessage.StudentNotFound);
            }
            return student;
        }

        public async Task<bool> RemoveStudent(Guid studentId)
        {
            if (studentId == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.StudentMessage.StudentNotFound);
            }
            var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(studentId) && s.DelFlg != true);
            if (student == null)
            {

                throw new BadHttpRequestException(MessageConstant.StudentMessage.StudentNotFound);
            }
            student.UpdDate = TimeUtils.GetCurrentSEATime();
            student.DelFlg = true;
            var account = await _unitOfWork.GetRepository<Account>()
                                            .SingleOrDefaultAsync(predicate: a => a.Id.Equals(student.AccountId) && a.DelFlg != true);
            if (account != null)
            {
                account.DelFlg = true;
                account.UpdDate = TimeUtils.GetCurrentSEATime();
                _unitOfWork.GetRepository<Account>().UpdateAsync(account);
            }
            _unitOfWork.GetRepository<Student>().UpdateAsync(student);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> UpdateStudent(Guid Id, UpdateStudentRequest request, Guid parentId)
        {
            if (Id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.StudentMessage.StudentNotFound);
            }
            var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(Id) && s.DelFlg != true);
            if (student == null)
            {
                throw new BadHttpRequestException(MessageConstant.StudentMessage.StudentNotFound);
            }

            if (parentId != Guid.Empty)
            {
                Parent parent = await _unitOfWork.GetRepository<Parent>().SingleOrDefaultAsync(predicate: p => p.Id.Equals(parentId) && p.DelFlg != true);
                if (parent == null)
                {
                    throw new BadHttpRequestException("Không tìm thấy dữ liệu của phụ huynh");
                }
                student.ParentId = parentId;
            }

            student.DateOfBirth = request.DateOfBirth ?? student.DateOfBirth;
            student.FirstName = string.IsNullOrEmpty(request.FirstName) ? student.FirstName : request.FirstName;
            student.LastName = string.IsNullOrEmpty(request.LastName) ? student.LastName : request.LastName;
            student.ImgUrl = string.IsNullOrEmpty(request.ImgUrl) ? student.ImgUrl : request.ImgUrl;
            student.UpdDate = TimeUtils.GetCurrentSEATime();

            _unitOfWork.GetRepository<Student>().UpdateAsync(student);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }
    }
}
