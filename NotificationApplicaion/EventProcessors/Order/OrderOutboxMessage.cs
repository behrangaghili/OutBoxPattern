namespace NotificationMicroservice.EventProcessors.Order
{
    public class OrderOutboxMessage
    {
        public int Id { get; set; }
        public int FromCityID { get; set; }
        public string Name { get; set; }
        public string CustomerMobileNo { get; set; }
    }
}
