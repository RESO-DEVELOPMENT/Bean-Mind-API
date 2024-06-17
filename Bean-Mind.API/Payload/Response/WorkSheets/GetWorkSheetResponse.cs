namespace Bean_Mind.API.Payload.Response.WorkSheets
{
    public class GetWorkSheetResponse
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid? ActivityId { get; set; }
        public Guid? SchoolId { get; set; }
        public Guid? WorksheetTemplateId { get; set; }
    }
}
