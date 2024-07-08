using Bean_Mind.API.Converter;
using Bean_Mind_Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bean_Mind.API.Payload.Request.Courses
{
    public class UpdateCourseRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public string? CourseCode { get; set; }
        public StatusEnum? Status { get; set; }
        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? StartDate { get; set; } // Nullable DateTime
        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? EndDate { get; set; } // Nullable DateTime
    }
}
