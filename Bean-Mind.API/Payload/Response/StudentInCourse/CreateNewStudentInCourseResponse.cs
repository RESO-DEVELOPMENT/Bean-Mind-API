namespace Bean_Mind.API.Payload.Response.StudentInCourse
{
    public class CreateNewStudentInCourseResponse
    {
        public Guid? StudentId { get; set; }
        public List<Guid>? CourseIds { get; set; }
    }
}
