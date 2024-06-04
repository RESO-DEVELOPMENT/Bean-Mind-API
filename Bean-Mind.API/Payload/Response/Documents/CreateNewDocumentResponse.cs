using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bean_Mind.API.Payload.Response.Documents
{
    public class CreateNewDocumentResponse
    {
        public Guid? Id { get; set; }
        [StringLength(50)]
        public string? Title { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public string? Url { get; set; } = null!;
        public Guid ActivityId { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }
        public bool? DelFlg { get; set; }
    }
}
