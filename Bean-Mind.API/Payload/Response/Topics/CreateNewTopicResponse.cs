using Bean_Mind_Data.Models;

namespace Bean_Mind.API.Payload.Response.Topics
{
    public class CreateNewTopicResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }
        public bool? DelFlg { get; set; }
        public Guid ChapterId { get; set; }

    }
}
