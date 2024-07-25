namespace Bean_Mind.API.Payload.Response.TeacherTeachables
{
    public class GetTeacherTeachableResponse
    {
        public Guid? Id { get; set; }
        public Guid? TeacherId { get; set; }
        public Guid? SubjectId { get; set; }
        public string? TeacherName { get; set; }
    }
}
