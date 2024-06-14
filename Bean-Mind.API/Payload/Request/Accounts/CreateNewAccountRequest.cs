using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Request
{
    public class CreateNewAccountRequest
    {
        [Required(ErrorMessage = "Username is missing")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Name is missing")]
        public string Password { get; set; }
        public CreateNewAccountRequest()
        {

        }
    }
}
