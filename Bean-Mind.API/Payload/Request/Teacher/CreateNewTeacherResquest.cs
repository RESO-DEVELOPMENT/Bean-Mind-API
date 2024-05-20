namespace Bean_Mind.API.Payload.Request.Teacher
{
    public class CreateNewTeacherResquest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ImgUrl { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
