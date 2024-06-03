using Bean_Mind.API.Payload.Response;
using Bean_Mind_Data.Models;

namespace Bean_Mind.API.Payload.Response.Students
{
    public class GetStudentResponse
    {
        public Guid? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ImgUrl { get; set; }

        public GetStudentResponse(Guid id, string firstName, string lastName, DateTime? dateOfBirth, string imgUrl) { 
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            ImgUrl = imgUrl;
        }
    }
}
