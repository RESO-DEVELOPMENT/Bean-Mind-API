using AutoMapper;
using Azure.Core;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.Curriculums;
using Bean_Mind.API.Payload.Response.Curriculums;
using Bean_Mind.API.Payload.Response.Schools;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensibility;
using System.Drawing;

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
            Curriculum newCurriculum = new Curriculum()
            {
                Id = Guid.NewGuid(),
                Title =createNewCurriculumRequest.Title,
                Description =createNewCurriculumRequest.Description,
                StartDate = TimeUtils.GetCurrentSEATime(),
                EndDate = TimeUtils.GetCurrentSEATime(),
                SchoolId = createNewCurriculumRequest.SchoolId,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false 

            };
            var school = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync( predicate: s => s.Id == createNewCurriculumRequest.SchoolId);

            if (school == null)
            {
               
                _logger.LogError($"SchoolId {createNewCurriculumRequest.SchoolId} not found.");
                
                return null;
            }
            await _unitOfWork.GetRepository<Curriculum>().InsertAsync(newCurriculum);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0 ;
            CreateNewCurriculumResponse createNewCurriculumResponse = null;
            if (isSuccessful)
            {
                createNewCurriculumResponse = new CreateNewCurriculumResponse()
                {
                    Title = newCurriculum.Title,
                    Description = newCurriculum.Description,
                    StartDate = newCurriculum.StartDate,
                    EndDate = newCurriculum.EndDate,
                    SchoolId = newCurriculum.SchoolId,
                    InsDate = TimeUtils.GetCurrentSEATime(),
                    UpdDate = TimeUtils.GetCurrentSEATime(),
                    DelFlg = newCurriculum.DelFlg


                };
            }

            return createNewCurriculumResponse;
        }

        public async Task<bool> deleteCurriculum(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.Curriculum.CurriculumNotFound);
            }
            var curriculum = await _unitOfWork.GetRepository<Curriculum>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(Id));
            if (curriculum == null)
            {

                throw new BadHttpRequestException(MessageConstant.Curriculum
                    .CurriculumNotFound);
            }
            curriculum.DelFlg = true;
            _unitOfWork.GetRepository<Curriculum>().UpdateAsync(curriculum);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
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
                 SchoolId = s.SchoolId,
                 InsDate = s.InsDate,
                 UpdDate = s.UpdDate,
                 DelFlg = s.DelFlg,
             },
             predicate: x => x.Id == Id
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
                  SchoolId = s.SchoolId,
                  InsDate = s.InsDate,
                  UpdDate = s.UpdDate,
                  DelFlg = s.DelFlg,
              },
              predicate: x => x.DelFlg == false,
              size: size,
              page: page);

            return curriculums;
        }

        public async Task<bool> updateCurriculum( Guid Id ,UpdateCurriculumRequest updateCurriculumRequest, Guid SchoolId)
        {
            if (Id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.Curriculum.CurriculumNotFound);
            }
            var curriculum  = await _unitOfWork.GetRepository<Curriculum>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(Id));
            if (curriculum == null)
            {
                throw new BadHttpRequestException(MessageConstant.Curriculum.CurriculumNotFound);
            }
            if (SchoolId != Guid.Empty)
            {
                School school = await _unitOfWork.GetRepository<School>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(SchoolId));
                if (school == null)
                {
                    throw new BadHttpRequestException(MessageConstant.School.SchoolNotFound);
                }
                curriculum.SchoolId = SchoolId;
            }


            curriculum.Title = string.IsNullOrEmpty(updateCurriculumRequest.Title.ToString()) ? curriculum.Title : updateCurriculumRequest.Title;
            curriculum.Description = string.IsNullOrEmpty(updateCurriculumRequest.Description.ToString()) ? curriculum.Description : updateCurriculumRequest.Description;
           
            
            curriculum.StartDate = TimeUtils.GetCurrentSEATime();
            curriculum.EndDate = TimeUtils.GetCurrentSEATime();
            


            _unitOfWork.GetRepository<Curriculum>().UpdateAsync(curriculum);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            return isSuccessful;
        }
    }
}
