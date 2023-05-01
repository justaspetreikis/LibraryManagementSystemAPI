using LibraryManagement.Application.Attributes;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Dto.Requests
{
    public class UpdateProfileImageRequest
    {
        [Required]
        [MaxFileSize(5 * 1024 * 1024)]
        public IFormFile Image { get; set; }
    }
}