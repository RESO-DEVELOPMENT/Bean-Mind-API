namespace Bean_Mind.API.Payload.Request.Students
{
    public class UpdateStudentRequest
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? ImgUrl { get; set; }
    }
}
