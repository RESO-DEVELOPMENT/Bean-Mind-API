namespace Bean_Mind.API.Payload.Response.Videos
{
    public class GetVideoResponse
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public string? Url { get; set; } = null!;
        public Guid? ActivityId { get; set; }
        public GetVideoResponse(Guid id, string title, string description, string url, Guid? activityId)
        {
            Id = id;
            Title = title;
            Description = description;
            Url = url;
            ActivityId = activityId;
        }
    }
}
