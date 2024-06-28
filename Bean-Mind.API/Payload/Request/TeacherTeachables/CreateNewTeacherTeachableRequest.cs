namespace Bean_Mind.API.Payload.Request.TeacherTeachables
{
    public class CreateNewTeacherTeachableRequest
    {
        public Guid TeacherId { get; set; }
        public Guid SubjectId { get; set; }
    }
}
