using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Dto.Requests
{
    public class UpdateFirstNameRequest
    {
        [Required]
        [RegularExpression(@"^\S+$", ErrorMessage = "First name cannot contain whitespace.")]
        public string FirstName { get; set; }
    }
}
