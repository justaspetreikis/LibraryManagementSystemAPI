using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Dto.Responses
{
    public class UserInformationResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Guid PersonId { get; set; }
        public Person Person { get; set; }
    }
}

