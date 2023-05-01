using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Dto.Requests
{
    public class UpdateStreetRequest
    {
        [Required]
        [RegularExpression(@"^\S+$", ErrorMessage = "Street cannot contain whitespace.")]
        public string Street { get; set; }
    }
}
