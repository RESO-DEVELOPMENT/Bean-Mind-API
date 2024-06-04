using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Response.Documents
{
    public class GetDocumentResponse
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public Guid? ActivityId { get; set; }
        public GetDocumentResponse(Guid id, string title, string description, string url, Guid? activityId)
        {
            Id = id;
            Title = title;
            Description = description;
            Url = url;
            ActivityId = activityId;
        }
    }
}
