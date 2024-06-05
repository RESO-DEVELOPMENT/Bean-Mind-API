namespace Bean_Mind.API.Payload.Request.WorkSheetTemplates
{
    public class UpdateWorkSheetTemplateRequest
    {
        public string? Title { get; set; }
        public int? EasyCount { get; set; }
        public int? MediumCount { get; set; }
        public int? HardCount { get; set; }
    }
}
