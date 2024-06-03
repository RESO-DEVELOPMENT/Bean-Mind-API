using System.ComponentModel.DataAnnotations;

namespace Bean_Mind.API.Payload.Request.Activities
{
    public class CreateNewActivityRequest
    {
        [Required(ErrorMessage = "Title is missing")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is missing")]
        public string Description { get; set; }

    }
}
