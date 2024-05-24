using Bean_Mind_Data.Models;

namespace Bean_Mind.API.Payload.Response.Teachers
{
    public class CreateNewTeacherResponse
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ImgUrl { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public School School { get; set; }
        public bool? DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }
    }
}
