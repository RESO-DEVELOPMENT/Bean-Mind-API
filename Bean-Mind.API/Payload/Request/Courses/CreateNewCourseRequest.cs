using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Request.Courses
{
    public class CreateNewCourseRequest
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [Required]
        public Guid CurriculumId { get; set; }

    }
}
