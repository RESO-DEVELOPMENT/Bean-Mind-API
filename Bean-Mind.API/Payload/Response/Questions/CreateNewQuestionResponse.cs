namespace Bean_Mind.API.Payload.Response.Question
{
    public class CreateNewQuestionResponse
    {
        public Guid? Id { get; set; }
        public string? Text { get; set; }
        public string? Image { get; set; }
        public int? OrderIndex { get; set; }
        public int? QuestionType { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }

    }
}

