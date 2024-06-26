namespace Bean_Mind.API.Payload.Response.Topics
{
    public class GetTopicResponse
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public Guid? ChapterId { get; set; }
        public Guid? SchoolId { get; set; }
        public GetTopicResponse(Guid id, string title, string description, Guid? chapterId, Guid? schoolId)
        {
            Id = id;
            Title = title;
            Description = description;
            ChapterId = chapterId;
            SchoolId = schoolId;
        }
    }
}
