namespace Bean_Mind.API.Payload.Response.Chapters
{
    public class GetChapterResponse
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public GetChapterResponse(Guid id, string? title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }
    }
}
