using Bean_Mind.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Request
{
    public class CreateNewAccountRequest
    {
        [Required(ErrorMessage = "Username is missing")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Name is missing")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is missing")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is missing")]
        public RoleEnum Role { get; set; }

        public CreateNewAccountRequest()
        {

        }
    }
}
