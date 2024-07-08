namespace Bean_Mind.API.Payload.Response.Courses
{
    public class GetCourseResponse
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? CourseCode { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Status {  get; set; }
        public Guid? CurriculumId { get; set; }
        public Guid? SchoolId { get; set; }
    }
}
