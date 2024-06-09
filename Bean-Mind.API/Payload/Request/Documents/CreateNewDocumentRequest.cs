namespace Bean_Mind.API.Payload.Request.Documents
{
    public class CreateNewDocumentRequest
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public IFormFile Url { get; set; } = null!;

        
    }
}
