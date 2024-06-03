using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Request.Activities
{
    public class UpdateActivityRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
