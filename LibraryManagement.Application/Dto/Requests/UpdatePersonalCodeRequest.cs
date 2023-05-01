using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Dto.Requests
{
    public class UpdatePersonalCodeRequest
    {
        [Required]
        [RegularExpression(@"^\S+$", ErrorMessage = "Personal code cannot contain whitespace.")]
        public string PersonalCode { get; set; }
    }
}
