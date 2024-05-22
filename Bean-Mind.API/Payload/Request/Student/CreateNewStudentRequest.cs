using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Request.Student
{
    public class CreateNewStudentRequest
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? ImgUrl { get; set; }
    }
}
