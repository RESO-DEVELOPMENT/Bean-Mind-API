namespace Bean_Mind.API.Payload.Response.WorkSheets
{
    public class CreateNewWorkSheetResponse
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid? ActivityId { get; set; }
        public Guid? SchoolId { get; set; }
        public Guid? WorksheetTemplateId { get; set; }
        public bool? DelFlg { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }
    }
}
