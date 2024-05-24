using Bean_Mind.API.Payload.Response;
using Bean_Mind_Data.Models;

namespace Bean_Mind.API.Payload.Response.Students
{
    public class GetStudentResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ImgUrl { get; set; }
        public Bean_Mind_Data.Models.Parent Parent { get; set; }
        public School School { get; set; }

        public GetStudentResponse(Guid id, string firstName, string lastName, DateTime? dateOfBirth, string imgUrl, Bean_Mind_Data.Models.Parent parent, School school) { 
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            ImgUrl = imgUrl;
            this.Parent = parent;
            this.School = school;
        }
    }
}
