using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.Dto.Requests
{
    public class UpdateHouseNumberRequest
    {
        [Required]
        [RegularExpression(@"^\S+$", ErrorMessage = "House number cannot contain whitespace.")]
        public string HouseNumber { get; set; }
    }
}
