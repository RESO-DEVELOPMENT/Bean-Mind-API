using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Response.Curriculums
{
    public class CreateNewCurriculumResponse
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? SchoolId { get; set; }
        public string? CurriculumCode { get; set; }
        public DateTime? InsDate { get; set; }
        public DateTime? UpdDate { get; set; }
        public bool? DelFlg { get; set; }
       
    }
    
}
