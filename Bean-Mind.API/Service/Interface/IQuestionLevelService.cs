using Bean_Mind.API.Payload.Request.QuestionLevels;
using Bean_Mind.API.Payload.Response.QuestionLevels;
using Bean_Mind_Data.Paginate;

namespace Bean_Mind.API.Service.Interface
{
    public interface IQuestionLevelService
    {
        public Task<CreateNewQuestionLevelResponse> CreateNewQuestionLevel(CreateNewQuestionLevelRequest request);
        public Task<IPaginate<GetQuestionLevelResponse>> GetListQuestionLevel(int page, int size);
        public Task<GetQuestionLevelResponse> GetQuestionLevelById(Guid id);
        public Task<bool> UpdateQuestionLevel(Guid id, UpdateQuestionLevelRequest request);
        public Task<bool> RemoveQuestionLevel(Guid id);
    }
}
