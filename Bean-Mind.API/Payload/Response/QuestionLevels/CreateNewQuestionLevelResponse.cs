namespace Bean_Mind.API.Payload.Response.QuestionLevels
{
    public class CreateNewQuestionLevelResponse
    {
        public Guid? Id { get; set; }
        public int? Level { get; set; }
        public Guid? SchoolId { get; set; }
        public bool? DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }
    }
}
