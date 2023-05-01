using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Dto.Requests
{
    public class UpdatePhoneNumberRequest
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
