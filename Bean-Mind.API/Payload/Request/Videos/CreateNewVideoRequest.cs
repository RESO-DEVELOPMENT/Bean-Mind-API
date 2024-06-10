namespace Bean_Mind.API.Payload.Request.Videos
{
    public class CreateNewVideoRequest
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IFormFile Url { get; set; } = null!;
    }
}
