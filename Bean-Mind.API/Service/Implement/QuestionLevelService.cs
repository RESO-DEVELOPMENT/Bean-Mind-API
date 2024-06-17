using AutoMapper;
using Bean_Mind.API.Constants;
using Bean_Mind.API.Payload.Request.QuestionLevels;
using Bean_Mind.API.Payload.Response.QuestionLevels;
using Bean_Mind.API.Service.Interface;
using Bean_Mind.API.Utils;
using Bean_Mind_Business.Repository.Interface;
using Bean_Mind_Data.Models;
using Bean_Mind_Data.Paginate;


namespace Bean_Mind.API.Service.Implement
{
    public class QuestionLevelService : BaseService<QuestionLevelService>, IQuestionLevelService
    {
        public QuestionLevelService(IUnitOfWork<BeanMindContext> unitOfWork, ILogger<QuestionLevelService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<CreateNewQuestionLevelResponse> CreateNewQuestionLevel(CreateNewQuestionLevelRequest request)
        {
            _logger.LogInformation($"Create new Question Level with {request.Level}");
            Guid? accountId = UserUtil.GetAccountId(_httpContextAccessor.HttpContext);
            var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
                predicate: s => s.Id.Equals(accountId) && s.DelFlg != true
                );
            if (account == null || account.SchoolId == null)
                throw new Exception("Account or SchoolId is null");

            QuestionLevel questionLevelExisted = await _unitOfWork.GetRepository<QuestionLevel>().SingleOrDefaultAsync(
                predicate: s => s.Level == request.Level && s.DelFlg != true && s.SchoolId.Equals(account.SchoolId)
                );
            if (questionLevelExisted != null)
            {
                throw new Exception("This level already exists");
            }

            QuestionLevel questionLevel = new QuestionLevel()
            {
                Id = Guid.NewGuid(),
                Level = request.Level,
                SchoolId = account.SchoolId.Value,
                InsDate = TimeUtils.GetCurrentSEATime(),
                UpdDate = TimeUtils.GetCurrentSEATime(),
                DelFlg = false
            };

            await _unitOfWork.GetRepository<QuestionLevel>().InsertAsync(questionLevel);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
            CreateNewQuestionLevelResponse response = null;
            if (isSuccessful)
            {
                response = new CreateNewQuestionLevelResponse()
                {
                    Id = questionLevel.Id,
                    Level = questionLevel.Level,
                    SchoolId = questionLevel.SchoolId,
                    InsDate = questionLevel.InsDate,
                    UpdDate = questionLevel.UpdDate,
                    DelFlg = questionLevel.DelFlg
                };
            }
            return response;
        }
        public async Task<IPaginate<GetQuestionLevelResponse>> GetListQuestionLevel(int page, int size)
        {
            var questionLevels = await _unitOfWork.GetRepository<QuestionLevel>().GetPagingListAsync(
                selector: s => new GetQuestionLevelResponse() 
                { 
                    Id = s.Id,
                    Level = s.Level,  
                    SchoolId= s.SchoolId,
                },
                predicate: s => s.DelFlg != true,
                page: page,
                size: size
                );
            if (questionLevels == null)
            {
                throw new BadHttpRequestException(MessageConstant.QuestionLevelMessage.QuestionLevelIsEmpty);
            }
            return questionLevels;
        }
        public async Task<GetQuestionLevelResponse> GetQuestionLevelById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.QuestionLevelMessage.QuestionLevelNotFound);
            }
            var questionLevel = await _unitOfWork.GetRepository<QuestionLevel>().SingleOrDefaultAsync(
                selector: s => new GetQuestionLevelResponse()
                {
                    Id = s.Id,
                    Level = s.Level,
                    SchoolId = s.SchoolId,
                },
                predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (questionLevel == null)
            {
                throw new BadHttpRequestException(MessageConstant.QuestionLevelMessage.QuestionLevelNotFound);
            }
            return questionLevel;
        }
        public async Task<bool> UpdateQuestionLevel(Guid id, UpdateQuestionLevelRequest request)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.QuestionLevelMessage.QuestionLevelNotFound);
            }
            var questionLevel = await _unitOfWork.GetRepository<QuestionLevel>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (questionLevel == null)
            {
                throw new BadHttpRequestException(MessageConstant.QuestionLevelMessage.QuestionLevelNotFound);
            }

            questionLevel.Level = request.Level == null ? questionLevel.Level : request.Level.Value;

            _unitOfWork.GetRepository<QuestionLevel>().UpdateAsync(questionLevel);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }
        public async Task<bool> RemoveQuestionLevel(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadHttpRequestException(MessageConstant.QuestionLevelMessage.QuestionLevelNotFound);
            }
            var questionLevel = await _unitOfWork.GetRepository<QuestionLevel>().SingleOrDefaultAsync(predicate: s => s.Id.Equals(id) && s.DelFlg != true);
            if (questionLevel == null)
            {
                throw new BadHttpRequestException(MessageConstant.QuestionLevelMessage.QuestionLevelNotFound);
            }
            questionLevel.DelFlg = true;
            questionLevel.UpdDate = TimeUtils.GetCurrentSEATime();

            _unitOfWork.GetRepository<QuestionLevel>().UpdateAsync(questionLevel);
            bool isSuccessful = await _unitOfWork.CommitAsync() > 0;

            return isSuccessful;
        }
    }
}
