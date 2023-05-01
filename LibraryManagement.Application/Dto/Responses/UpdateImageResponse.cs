namespace LibraryManagement.Application.Dto.Responses
{
    public class UpdateImageResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] ImageBytes { get; set; }
        public string ContentType { get; set; }
    }
}
