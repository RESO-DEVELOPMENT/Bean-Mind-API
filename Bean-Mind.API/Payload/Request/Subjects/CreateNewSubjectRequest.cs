using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Request.Subjects
{
    public class CreateNewSubjectRequest
    {
        public string Title { get; set; } = null!;
        public string SubjectCode { get; set; }
        public string Description { get; set; } = null!;
        public string? SubjectCode { get; set; }
    }
}
