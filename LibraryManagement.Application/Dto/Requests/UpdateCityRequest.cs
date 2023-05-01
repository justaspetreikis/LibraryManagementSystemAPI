using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Dto.Requests
{
    public class UpdateCityRequest
    {
        [Required]
        [RegularExpression(@"^\S+$", ErrorMessage = "City cannot contain whitespace.")]
        public string City { get; set; }
    }
}
