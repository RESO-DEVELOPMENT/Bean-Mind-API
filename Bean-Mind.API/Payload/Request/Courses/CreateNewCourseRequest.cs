using Bean_Mind_Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Request.Courses
{
    public class CreateNewCourseRequest
    {
        [Required]
        [StringLength(50)]
        public string? Title { get; set; }

        [Required]
        [StringLength(50)]
        public string CourseCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(50)]
        public string? CourseCode { get; set; }
        [Required]
        public StatusEnum Status { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

    }
}
