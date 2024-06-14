namespace Bean_Mind.API.Payload.Response.Subjects
{
    public class GetSubjectResponse
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid? CourseId { get; set; }
        public GetSubjectResponse(Guid id, string? title, string description, Guid? courseId)
        {
            Id = id;
            Title = title;
            Description = description;
            CourseId = courseId;
        }
    }
}
