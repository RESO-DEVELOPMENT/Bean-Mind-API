using Bean_Mind.API.Payload.Response;
using Bean_Mind_Data.Models;

namespace Bean_Mind.API.Payload.Response.Student
{
    public class GetStudentResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Parent Parent { get; set; }

        public GetStudentResponse(Guid id, string firstName, string lastName, DateTime? dateOfBirth, Parent parent) { 
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Parent = parent;
        }
    }
}
