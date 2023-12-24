namespace OrderMicroservice.Models
{
    public class OutboxEventModel
    {
        public int Id { get; set; }
        public Guid EventId { get; set; }
        public string EventType { get; set; }
        public string EventData { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string? Payload { get; set; }
    }
}
