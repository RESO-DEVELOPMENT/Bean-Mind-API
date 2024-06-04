using Bean_Mind_Data.Enums;

namespace Bean_Mind.API.Payload.Request.Courses
{
    public class UpdateCourseRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public StatusEnum? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
