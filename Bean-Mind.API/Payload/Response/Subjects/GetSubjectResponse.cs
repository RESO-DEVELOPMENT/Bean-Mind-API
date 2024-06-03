using Bean_Mind_Data.Models;

namespace Bean_Mind.API.Payload.Response.Subjects
{
    public class GetSubjectResponse
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public GetSubjectResponse(Guid id, string? title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }
    }
}
