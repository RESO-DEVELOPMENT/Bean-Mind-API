using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Request.Schools
{
    public class CreateNewSchoolRequest
    {
        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Logo { get; set; }

        public string? Description { get; set; }

        public string? Email { get; set; } = null!;
        [Required(ErrorMessage = "Username is missing")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Name is missing")]
        public string Password { get; set; }
    }
}
