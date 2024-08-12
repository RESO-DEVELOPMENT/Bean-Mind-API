using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Request.Teachers
{
    public class CreateNewTeacherResquest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public IFormFile ImgUrl { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [Required(ErrorMessage = "Username is missing")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Name is missing")]
        public string Password { get; set; }
    }
}
