namespace Bean_Mind.API.Payload.Response.Teacher
{
    public class TeacherResponse
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? ImgUrl { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public Guid SchoolId { get; set; }

    }
}
