using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.School;
using Bean_Mind.API.Payload.Request.Student;
using Bean_Mind.API.Payload.Response.School;
using Bean_Mind.API.Payload.Response.Student;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
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

        public async Task<CreateNewStudentResponse> CreateNewStudent(CreateNewStudentRequest request, Guid schoolId, Guid parentId)
        {
            _logger.LogInformation($"Create new Student with {request.FirstName}  {request.LastName}");
            School school = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(schoolId));
            if (school == null)
            {
                throw new BadHttpRequestException(MessageConstant.School.SchoolNotFound);
            }
            Parent parent = await _unitOfWork.GetRepository<Parent>().SingleOrDefaultAsync(predicate: p => p.Id == parentId);
            if (parent == null)
            {
                throw new BadHttpRequestException("Không tìm thấy dữ liệu của phụ huynh");
            }
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
                SchoolId = schoolId,
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
            var students = await _unitOfWork.GetRepository<Student>().GetPagingListAsync(
                selector: s => new GetStudentResponse(s.Id, s.FirstName, s.LastName, s.DateOfBirth, s.Parent),
                predicate: s => s.DelFlg != true,
                include: s => s.Include(s => s.Parent),
                page : page,
                size : size
                );
            return students;
        }

        public async Task<GetStudentResponse> getStudentById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.Student.StudentNotFound);
            }
            var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(
                selector: s => new GetStudentResponse(s.Id, s.FirstName, s.LastName, s.DateOfBirth, s.Parent),
                predicate: s => s.Id.Equals(id) && s.DelFlg != true,
                include: s => s.Include(s => s.Parent));
            return student;
        }

        public async Task<bool> RemoveStudent(Guid studentId)
        {
            if (studentId == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.Student.StudentNotFound);
            }
            var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(studentId));
            if (student == null)
            {

                throw new BadHttpRequestException(MessageConstant.Student.StudentNotFound);
            }
            student.DelFlg = true;
            _unitOfWork.GetRepository<Student>().UpdateAsync(student);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<bool> UpdateStudent(Guid Id, CreateNewStudentRequest request, Guid schoolId, Guid parentId)
        {
            if (Id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.Student.StudentNotFound);
            }
            var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(Id));
            if (student == null)
            {
                throw new BadHttpRequestException(MessageConstant.Student.StudentNotFound);
            }
            if (schoolId != Guid.Empty)
            {
                School school = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(schoolId));
                if (school == null)
                {
                    throw new BadHttpRequestException(MessageConstant.School.SchoolNotFound);
                }
                student.SchoolId = schoolId;
            }
            if (parentId != Guid.Empty)
            {
                Parent parent = await _unitOfWork.GetRepository<Parent>().SingleOrDefaultAsync(predicate: p => p.Id == parentId);
                if (parent == null)
                {
                    throw new BadHttpRequestException("Không tìm thấy dữ liệu của phụ huynh");
                }
                student.ParentId = parentId;
            }
            
            student.DateOfBirth = string.IsNullOrEmpty(request.DateOfBirth.ToString()) ? student.DateOfBirth : request.DateOfBirth;
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
