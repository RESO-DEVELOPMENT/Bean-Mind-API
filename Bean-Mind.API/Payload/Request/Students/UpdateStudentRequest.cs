using Bean_Mind.API.Converter;
using System.Text.Json.Serialization;

namespace Bean_Mind.API.Payload.Request.Students
{
    public class UpdateStudentRequest
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        [JsonConverter(typeof(NullableDateTimeConverter))]

        public DateTime? DateOfBirth { get; set; }

        public IFormFile? ImgUrl { get; set; }
    }
}
