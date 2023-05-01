using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Dto.Requests
{
    public class UpdateLastNameRequest
    {
        [Required]
        [RegularExpression(@"^\S+$", ErrorMessage = "Last name cannot contain whitespace.")]
        public string LastName { get; set; }
    }
}
