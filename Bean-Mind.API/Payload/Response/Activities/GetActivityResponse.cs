namespace Bean_Mind.API.Payload.Response.Activities
{
    public class GetActivityResponse
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid? TopicId { get; set; }
    }
}
