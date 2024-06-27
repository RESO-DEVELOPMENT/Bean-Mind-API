using AutoMapper;

using Bean_Mind.API.Payload.Request.TeacherTeachables;
using Bean_Mind.API.Payload.Response.TeacherTeachables;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using static Bean_Mind.API.Constants.MessageConstant;


namespace Bean_Mind.API.Service.Implement
{
    public class TeacherTeachableService :BaseService<TeacherTeachableService>,ITeacherTeachableService
    {
        public TeacherTeachableService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<TeacherTeachableService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<GetTeacherTeachableResponse> CreateTeacherTeachable(CreateNewTeacherTeachableRequest request)
        {
            _logger.LogInformation($"Create new teacher-teachable relationship with TeacherID: {request.TeacherId} and SubjectID: {request.SubjectId}");

            
            var subjectExist = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(request.SubjectId) && s.DelFlg != true
            );
            if (subjectExist == null)
            {
                throw new BadHttpRequestException(TeacherTeachableMessage.InvalidInputData);
            }

            
            var teacherExist = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(request.TeacherId) && s.DelFlg != true
            );
            if (teacherExist == null)
            {
                throw new BadHttpRequestException(TeacherTeachableMessage.InvalidInputData);
            }

            
            var existingRelationship = await _unitOfWork.GetRepository<TeacherTeachable>().SingleOrDefaultAsync(
                predicate: tt => tt.TeacherId.Equals(request.TeacherId) && tt.SubjectId.Equals(request.SubjectId) && tt.DelFlg != true
            );
            if (existingRelationship != null)
            {
                throw new BadHttpRequestException(TeacherTeachableMessage.CreateFailed);
            }

            TeacherTeachable teacherTeachable = new TeacherTeachable()
            {
                Id = Guid.NewGuid(),
                TeacherId = request.TeacherId,
                SubjectId = request.SubjectId,
                UpdDate = TimeUtils.GetCurrentSEATime(),
                InsDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false,
            };

            await _unitOfWork.GetRepository<TeacherTeachable>().InsertAsync(teacherTeachable);
            var success = await _unitOfWork.CommitAsync() > 0;
            if (!success)
            {
                throw new BadHttpRequestException(TeacherTeachableMessage.CreateFailed);
            }

            return new GetTeacherTeachableResponse
            {
                Id = teacherTeachable.Id,
                TeacherId = teacherTeachable.TeacherId,
                SubjectId = teacherTeachable.SubjectId,
            };
        }


        public async Task<IPaginate<GetTeacherTeachableResponse>> GetAllTeacherTeachables(int page, int size)
        {
            var teacherTeachables = await _unitOfWork.GetRepository<TeacherTeachable>().GetPagingListAsync(
                selector: x => new GetTeacherTeachableResponse()
                { 
                    Id = x.Id,
                    TeacherId = x.TeacherId,
                    SubjectId = x.SubjectId
                },
                predicate: x => x.Teacher.DelFlg == false && x.Subject.DelFlg == false,
                size: size,
                page: page
            );
            return teacherTeachables;
        }

        public async Task<IPaginate<GetTeacherTeachableResponse>> GetTeacherTeachablesByTeacher(Guid teacherId, int page, int size)
        {
            var teacherTeachables = await _unitOfWork.GetRepository<TeacherTeachable>().GetPagingListAsync(
                selector: x => new GetTeacherTeachableResponse()
                {
                    Id = x.Id,
                    TeacherId = x.TeacherId,
                    SubjectId = x.SubjectId
                },
                predicate: x => x.TeacherId.Equals(teacherId) && x.Teacher.DelFlg != true && x.Subject.DelFlg != true
            );

            return teacherTeachables;
        }

        public async Task<IPaginate<GetTeacherTeachableResponse>> GetTeacherTeachablesBySubject(Guid subjectId, int page, int size)
        {
            var teacherTeachables = await _unitOfWork.GetRepository<TeacherTeachable>().GetPagingListAsync(
                selector: x => new GetTeacherTeachableResponse()
                {
                    Id = x.Id,
                    TeacherId = x.TeacherId,
                    SubjectId = x.SubjectId
                },
                predicate: x => x.SubjectId.Equals(subjectId) && x.Teacher.DelFlg != true && x.Subject.DelFlg != true
            );

            return teacherTeachables;
        }



        public async Task<bool> UpdateTeacherTeachable(Guid id, UpdateTeacherTeachableRequest request)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(TeacherTeachableMessage.NotFound);
            }

            var teacherTeachable = await _unitOfWork.GetRepository<TeacherTeachable>().SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(id) && x.DelFlg != true
            );

            if (teacherTeachable == null)
            {
                throw new BadHttpRequestException(TeacherTeachableMessage.NotFound);
            }

            if (request.TeacherId.HasValue && request.TeacherId != Guid.Empty && request.TeacherId != teacherTeachable.TeacherId)
            {
                var existingTeacherTeachable = await _unitOfWork.GetRepository<TeacherTeachable>().SingleOrDefaultAsync(
                    predicate: x => x.TeacherId.Equals(request.TeacherId.Value) && x.SubjectId.Equals(teacherTeachable.SubjectId) && x.Id != id && x.DelFlg != true
                );

                if (existingTeacherTeachable != null)
                {
                    throw new BadHttpRequestException("Duplicate Teacher-Subject combination found");
                }

                var teacher = await _unitOfWork.GetRepository<Teacher>().SingleOrDefaultAsync(
                    predicate: t => t.Id.Equals(request.TeacherId.Value) && t.DelFlg != true 
                );

                if (teacher == null)
                {
                    throw new BadHttpRequestException(TeacherTeachableMessage.NotFound);
                }

                teacherTeachable.TeacherId = request.TeacherId.Value;
            }

            if (request.SubjectId.HasValue && request.SubjectId != Guid.Empty && request.SubjectId != teacherTeachable.SubjectId)
            {
                var existingTeacherTeachable = await _unitOfWork.GetRepository<TeacherTeachable>().SingleOrDefaultAsync(
                    predicate: x => x.TeacherId.Equals(teacherTeachable.TeacherId) && x.SubjectId.Equals(request.SubjectId.Value) && x.Id != id && x.DelFlg != true
                );

                if (existingTeacherTeachable != null)
                {
                    throw new BadHttpRequestException("Duplicate Teacher-Subject combination found");
                }

                var subject = await _unitOfWork.GetRepository<Subject>().SingleOrDefaultAsync(
                    predicate: s => s.Id.Equals(request.SubjectId.Value) && s.DelFlg != true 
                );

                if (subject == null)
                {
                    throw new BadHttpRequestException(TeacherTeachableMessage.NotFound);
                }

                teacherTeachable.SubjectId = request.SubjectId.Value;
            }

            teacherTeachable.UpdDate = TimeUtils.GetCurrentSEATime();

            _unitOfWork.GetRepository<TeacherTeachable>().UpdateAsync(teacherTeachable);
            var success = await _unitOfWork.CommitAsync() > 0;

            if (!success)
            {
                throw new BadHttpRequestException(TeacherTeachableMessage.UpdateFailed);
            }

            return success;
        }





        public async Task<bool> RemoveTeacherTeachable(Guid id)
        {
            var teacherTeachable = await _unitOfWork.GetRepository<TeacherTeachable>().SingleOrDefaultAsync(
                predicate: x => x.Id == id && x.Teacher.DelFlg != true && x.Subject.DelFlg != true
            );

            if (teacherTeachable == null)
                throw new BadHttpRequestException("Teacher-Teachable relationship not found");

            teacherTeachable.DelFlg = true;
            teacherTeachable.UpdDate = TimeUtils.GetCurrentSEATime();

            _unitOfWork.GetRepository<TeacherTeachable>().UpdateAsync(teacherTeachable);
            var success = await _unitOfWork.CommitAsync() > 0;

            return success;
        }

    }
}
