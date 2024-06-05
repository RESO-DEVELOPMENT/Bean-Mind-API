namespace Bean_Mind.API.Payload.Response.QuestionAnswers
{
    public class GetQuestionAnswerResponse
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
