using Bean_Mind.API.Converter;
using System.Text.Json.Serialization;

namespace Bean_Mind.API.Payload.Request.Teachers
{
    public class UpdateTecherRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? DateOfBirth { get; set; }
        public IFormFile? ImgUrl { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public void TrimString()
        {
            FirstName = FirstName?.Trim();
            
        }
    }
}
