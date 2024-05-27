using System;

namespace Bean_Mind.API.Payload.Request.Curriculums
{
    public class UpdateCurriculumRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
    }
}
