using Bean_Mind.API.Payload.Request.QuestionAnswers;
using Bean_Mind.API.Payload.Request.Questions;
using Bean_Mind.API.Payload.Response.Question;
using Bean_Mind.API.Payload.Response.QuestionAnswers;
using Bean_Mind.API.Payload.Response.Questions;
using Bean_Mind_Data.Enums;

namespace Bean_Mind.API.Service.Interface
{
    public interface IQuestionService
    {
        Task<CreateNewQuestionResponse> CreateNewQuestion(IFormFile img, string text, int orderIndex, List<CreateNewQuestionAnswerRequest> answerRequests, QuestionType questionType, Guid questionLevelId);
        Task<List<GetQuestionResponse>> GetAllQuestion(int page , int size);
        Task<List<GetQuestionAnswerResponse>> GetQuestionAnswersByQuestionId(Guid id);
        //Task<bool> UpdateQuestion(Guid id, UpdateQuestionRequest request, Guid questionLevelId);
        Task<bool> RemoveQuestion(Guid id);
    }
}
