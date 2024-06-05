namespace Bean_Mind.API.Payload.Request.WorkSheetTemplates
{
    public class CreateNewWorkSheetTemplateRequest
    {
        public string Title { get; set; } = null!;
        public int EasyCount { get; set; }
        public int MediumCount { get; set; }
        public int HardCount { get; set; }
    }
}
