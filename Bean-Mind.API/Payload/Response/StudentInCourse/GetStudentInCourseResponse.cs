namespace Bean_Mind.API.Payload.Response.StudentInCourse
{
    public class GetStudentInCourseResponse
    {
        public Guid? Id { get; set; }
        public Guid? StudentId { get; set; }
        public List<Guid>? CourseId { get; set; }
    }
}
