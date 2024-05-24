using Bean_Mind_Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Response.Parents
{
    public class CreateNewParentResponse
    {
        public Guid Id { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public string Address { get; set; }

        public DateTime? InsDate { get; set; }

        public DateTime? UpdDate { get; set; }

        public bool? DelFlg { get; set; }
        public string Message { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
