using Bean_Mind.API.Payload.Request.QuestionAnswers;
using Bean_Mind_Data.Enums;

namespace Bean_Mind.API.Payload.Request.Questions
{
    public class CreateNewQuestionRequest
    {
        public string Text { get; set; }
        public IFormFile? Image { get; set; }
        public int OrderIndex { get; set; }
        public string Answers { get; set; }
        public QuestionType QuestionType { get; set; }

    }
}