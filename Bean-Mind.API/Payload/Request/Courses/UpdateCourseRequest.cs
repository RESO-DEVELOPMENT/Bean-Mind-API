using Bean_Mind_Data.Enums;
using System.Text.Json.Serialization;

namespace Bean_Mind.API.Payload.Request.Courses
{
    public class UpdateCourseRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusEnum? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
