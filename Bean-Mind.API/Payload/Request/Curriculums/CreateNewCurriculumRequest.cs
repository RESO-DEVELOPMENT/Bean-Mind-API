using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Request.Curriculums
{
    public class CreateNewCurriculumRequest
    {
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid SchoolId { get; set; }
       
        
    }
}
