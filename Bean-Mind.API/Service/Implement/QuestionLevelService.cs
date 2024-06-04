using AutoMapper;
using Bean_Mind.API.Payload.Request.QuestionLevels;
using Bean_Mind.API.Payload.Response.QuestionLevels;
using Bean_Mind.API.Service.Interface;
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

        }
        public async Task<IPaginate<GetQuestionLevelResponse>> GetListQuestionLevel(int page, int size)
        {

        }
        public async Task<GetQuestionLevelResponse> GetQuestionLevelById(Guid id)
        {

        }
        public async Task<bool> UpdateQuestionLevel(Guid id, UpdateQuestionLevelRequest request)
        {

        }
        public async Task<bool> RemoveQuestionLevel(Guid id)
        {

        }
    }
}
