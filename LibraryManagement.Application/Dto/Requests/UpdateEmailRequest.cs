using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Dto.Requests
{
    public class UpdateEmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
