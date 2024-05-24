using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Request.Parent
{
    public class CreateNewParentResquest
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public string Address { get; set; }
    }
}
