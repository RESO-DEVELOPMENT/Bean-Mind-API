using Bean_Mind_Data.Enums;

namespace Bean_Mind.API.Payload.Request.QuestionAnswers
{
    public class UpdateQuestionRequest
    {
        public string Text { get; set; }
        public IFormFile Image { get; set; }
        public int OrderIndex { get; set; }
        public QuestionType QuestionType { get; set; }
        public Guid QuestionLevelId { get; set; }
      
    }
}
