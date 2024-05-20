using AutoMapper;
using Bean_Mind.API.Payload.Request.School;
using Bean_Mind.API.Payload.Response.School;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
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
            var school = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(Id));
            school.DelFlg = true;
            _unitOfWork.GetRepository<School>().UpdateAsync(school);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }

        public async Task<IPaginate<GetSchoolResponse>> getListSchool(int page, int size)
        {
            var schools = await _unitOfWork.GetRepository<School>().GetPagingListAsync(
                selector: s => new GetSchoolResponse(s.Id, s.Name, s.Address, s.Phone, s.Email, s.Logo, s.Description),
                predicate: s => s.DelFlg != true,
                page: page, size: size);
            return schools;

        }


        public async Task<GetSchoolResponse> getSchoolById(Guid id)
        {
            var school = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync(
                selector: s => new GetSchoolResponse(s.Id, s.Name, s.Address, s.Phone, s.Email, s.Logo, s.Description),
                predicate: s => s.Id.Equals(id) && s.DelFlg != true
        );
            return school;
        }

        public async Task<bool> updateSchool(CreateNewSchoolRequest createNewSchoolRequest, Guid Id)
        {
            var school = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(Id));
            if (school == null)
            {
                return false;
            }
            school.Name = string.IsNullOrEmpty(createNewSchoolRequest.Name) ? school.Name : createNewSchoolRequest.Name;
            school.Address = string.IsNullOrEmpty(createNewSchoolRequest.Address) ? school.Address : createNewSchoolRequest.Address;
            school.Phone = string.IsNullOrEmpty(createNewSchoolRequest.Phone) ? school.Phone : createNewSchoolRequest.Phone;
            school.Logo = string.IsNullOrEmpty(createNewSchoolRequest.Logo) ? school.Logo : createNewSchoolRequest.Logo;
            school.Description = string.IsNullOrEmpty(createNewSchoolRequest.Description) ? school.Description : createNewSchoolRequest.Description;
            school.Email = string.IsNullOrEmpty(createNewSchoolRequest.Email) ? school.Email : createNewSchoolRequest.Email;
            school.UpdDate = TimeUtils.GetCurrentSEATime();

            _unitOfWork.GetRepository<School>().UpdateAsync(school);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
