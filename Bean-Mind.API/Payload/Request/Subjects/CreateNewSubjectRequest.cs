using Bean_Mind_Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Request.Subjects
{
    public class CreateNewSubjectRequest
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
