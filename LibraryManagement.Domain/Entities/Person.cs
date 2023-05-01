
namespace LibraryManagement.Domain.Entities
{
    public class Person
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PersonalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }
        public Guid ProfileImageId { get; set; }
        public virtual ProfileImage ProfileImage { get; set; }
    }
}
