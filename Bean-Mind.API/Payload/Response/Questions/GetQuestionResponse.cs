using Bean_Mind.API.Payload.Response.QuestionAnswers;
using Bean_Mind_Data.Enums;

namespace Bean_Mind.API.Payload.Response.Questions
{
    public class GetQuestionResponse
    {
        public Guid Id { get; set; }
        public string? Text { get; set; }
        public string? Image { get; set; }
        public int? OrderIndex { get; set; }
        public QuestionType? QuestionType { get; set; }
        public Guid? QuestionLevelId { get; set; }
        public Guid? SchoolId { get; set; }
        public List<GetQuestionAnswerResponse>? Answers { get; set; }
    }
}
