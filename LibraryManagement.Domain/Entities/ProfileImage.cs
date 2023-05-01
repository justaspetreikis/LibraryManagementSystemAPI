namespace LibraryManagement.Domain.Entities
{
    public class ProfileImage
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] ImageBytes { get; set; }
        public string ContentType { get; set; }
    }
}
