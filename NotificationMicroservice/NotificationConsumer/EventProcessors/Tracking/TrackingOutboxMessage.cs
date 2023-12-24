namespace NotificationMicroservice.EventProcessors.Tracking
{
    public class TrackingOutboxMessage
    {
        public int Id { get; set; }
        public string NewStatus { get; set; }
    }
}
