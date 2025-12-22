namespace Fota.Models.DTOs
{
    public class MessageGetDTO
    {
        public long MessageId { get; set; }
        public int TopicId { get; set; }
        public int PublisherId { get; set; }
        public string Payload { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

    }

    public class MessagePostDTO
    {
    

        public int TopicId { get; set; }          // للـ DB
        public string TopicName { get; set; }     // للـ MQTT
        public int PublisherId { get; set; }      // مين اللي بعت
        public string Payload { get; set; }       // البيانات
        public DateTime CreatedAt { get; set; }

    }
}
