using Bean_Mind.API.Converter;
using System;
using System.Text.Json.Serialization;

namespace Bean_Mind.API.Payload.Request.Curriculums
{
    public class UpdateCurriculumRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? StartDate { get; set; }
        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? EndDate { get; set; }
        
    }
}
