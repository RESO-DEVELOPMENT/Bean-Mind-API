namespace Bean_Mind.API.Payload.Response.Subjects
{
    public class GetSubjectResponse
    {
        public Guid? Id { get; set; }
        public string? SubjectCode { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid? CourseId { get; set; }
        public Guid? SchoolId { get; set; }

        public GetSubjectResponse(Guid id, string? title, string? subjectCode,string description, Guid? courseId, Guid? schoolId)
        {
            Id = id;
            Title = title;
            SubjectCode = subjectCode;
            Description = description;
            CourseId = courseId;
            SchoolId = schoolId;
        }
    }
}
