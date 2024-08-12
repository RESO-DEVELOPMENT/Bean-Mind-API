using Bean_Mind.API.Payload.Request.QuestionAnswers;
using Bean_Mind.API.Payload.Response.Question;
using Bean_Mind.API.Payload.Response.QuestionAnswers;
using Bean_Mind.API.Payload.Response.Questions;
using Bean_Mind_Data.Enums;
using Bean_Mind_Data.Models;

namespace Bean_Mind.API.Service.Interface
{
    public interface IQuestionService
    {
        Task<CreateNewQuestionResponse> CreateNewQuestion(IFormFile img, string text, int orderIndex, List<CreateNewQuestionAnswerRequest> answerRequests, QuestionType questionType, Guid questionLevelId);
        Task<List<GetQuestionResponse>> GetAllQuestion(int page, int size);
        Task<List<GetQuestionAnswerResponse>> GetQuestionAnswersByQuestionId(Guid id);
        Task<bool> RemoveQuestion(Guid id);

        // Phương thức cập nhật chi tiết câu hỏi
        Task<Question> UpdateQuestionDetails(Guid questionId, IFormFile img, string text, int orderIndex, QuestionType questionType, Guid questionLevelId);

        // Phương thức cập nhật câu trả lời của câu hỏi
        Task UpdateQuestionAnswers(Guid questionId, List<UpdateQuestionAnswerRequest> answerRequests);
    }
}
