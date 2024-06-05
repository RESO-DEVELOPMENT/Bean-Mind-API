using System.ComponentModel.DataAnnotations.Schema;

namespace Bean_Mind.API.Payload.Response.WorkSheetTemplates
{
    public class CreateNewWorkSheetTemplateResponse
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public int? EasyCount { get; set; }
        public int? MediumCount { get; set; }
        public int? HardCount { get; set; }
        public Guid? SubjectId { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }
        public bool? DelFlg { get; set; }
    }
}
