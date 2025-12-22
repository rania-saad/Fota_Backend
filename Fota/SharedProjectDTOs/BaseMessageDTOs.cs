using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProjectDTOs.BaseMessages
{
    public class BaseMessageGetDto
    {
        public int Id { get; set; }
        public string MessageType { get; set; } = "Standard";
        public string? Description { get; set; }
      //  public byte[]? HexFileContent { get; set; }

        public string? HexFileContent { get; set; }
        public string? HexFileName { get; set; }
        public string? Version { get; set; }
        public string Status { get; set; } = "Draft";

        public DateTime? ApprovedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime? RejectedAt { get; set; }
        public string? RejectionReason { get; set; }

        public int TopicId { get; set; }
        public string? TopicName { get; set; }

        public int UploaderId { get; set; }
        public string? UploaderName { get; set; }

        public int? PublisherId { get; set; }
        public string? PublisherName { get; set; }

        public List<int> DeliveryIds { get; set; } = new();
    }

    public class BaseMessageListDto
    {
        public int Id { get; set; }
        public string MessageType { get; set; } = "Standard";
        public string? Description { get; set; }
        public string? HexFileName { get; set; }
        public string? Version { get; set; }

        public string Status { get; set; } = "Draft";
        public string? TopicName { get; set; }
        public string? UploaderName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
    }

    public class BaseMessageCreateDto
    {
        public string MessageType { get; set; } = "Standard";
        public string? Description { get; set; }
        //public byte[]? HexFileContent { get; set; }
        public string? HexFileContent { get; set; }

        public string? HexFileName { get; set; }
        public string? Version { get; set; }

        public string Status { get; set; } 

        public int TopicId { get; set; }
        public int UploaderId { get; set; }
    }
    public class BaseMessageUpdateDto
    {
        public string? MessageType { get; set; }
        public string? Description { get; set; }
        public string? HexFileContent { get; set; }

        public string? HexFileName { get; set; }
        public string? Version { get; set; }
        public int? TopicId { get; set; }
    }
    public class BaseMessageApproveDto
    {
        public int ApprovedById { get; set; }
    }
    public class BaseMessageRejectDto
    {
        public string RejectionReason { get; set; } = null!;
    }
    public class BaseMessagePublishDto
    {
        public int PublisherId { get; set; }
    }
    public class BaseMessageWithFileDto
    {
        public int Id { get; set; }
        public string MessageType { get; set; } = "Standard";
        public string? Description { get; set; }
        public string Status { get; set; } = "Draft";
        public string? TopicName { get; set; }
        public string? UploaderName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
    }
}
