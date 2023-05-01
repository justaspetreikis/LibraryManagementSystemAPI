using LibraryManagement.Application.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Dto.Requests
{
    public class SignUpRequest
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$")]
        public string Password { get; set; }

        [Required]
        [MaxFileSize(5 * 1024 * 1024)]
        public IFormFile Image { get; set; }
    }
}
