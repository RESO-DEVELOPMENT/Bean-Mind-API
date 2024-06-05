namespace Bean_Mind.API.Payload.Response.WorkSheetTemplates
{
    public class GetWorkSheetTemplateResponse
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public int? EasyCount { get; set; }
        public int? MediumCount { get; set; }
        public int? HardCount { get; set; }
        public Guid? SubjectId { get; set; }

        public GetWorkSheetTemplateResponse(Guid? id, string? title, int? easyCount, int? mediumCount, int? hardCount, Guid? subjectId)
        {
            Id = id;
            Title = title;
            EasyCount = easyCount;
            MediumCount = mediumCount;
            HardCount = hardCount;
            SubjectId = subjectId;
        }
    }
}
