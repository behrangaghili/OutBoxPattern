using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationMicroservice.Entities
{
    [Table("Messages", Schema = "Notification")]
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string Type { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int TemplateID { get; set; }
        public string Body { get; set; }
        public string? Source { get; set; }
        public DateTime ReceivedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
