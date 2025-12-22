namespace Fota.Models.DTOs
{
    public class PublisherGetDTO
    {
        public int PublisherId { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
       // public ICollection<Message> Messages { get; set; } = new List<Message>();
    }

    public class PublisherPostDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        // public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
