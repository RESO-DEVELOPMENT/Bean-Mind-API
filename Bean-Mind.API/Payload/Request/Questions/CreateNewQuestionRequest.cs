using Bean_Mind.API.Payload.Request.QuestionAnswers;
using Bean_Mind_Data.Enums;

namespace Bean_Mind.API.Payload.Request.Questions
{
    public class CreateNewQuestionRequest
    {
        public string? Text { get; set; }
        public string? Image { get; set; }
        public int OrderIndex { get; set; }
        public List<CreateNewQuestionAnswerRequest>? Answers { get; set; }
        public QuestionType? QuestionType { get; set; }

    }
}