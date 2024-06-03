using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Response.Documents
{
    public class GetDocumentResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Url { get; set; } = null!;

        public GetDocumentResponse(Guid id, string? title, string description, string? url)
        {
            Id = id;
            Title = title;
            Description = description;
            Url = url;
        }
    }
}
