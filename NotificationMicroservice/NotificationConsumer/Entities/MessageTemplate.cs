using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NotificationMicroservice.Entities
{
    [Table("MessageTemplates", Schema = "Notification")]
    public class MessageTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string Name { get; set; }
        public string Template { get; set; }
    }
}
