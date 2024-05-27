namespace Bean_Mind.API.Payload.Request.Courses
{
    public class UpdateCourseRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? UpdDate { get; set; }
    }
}
