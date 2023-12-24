namespace DispatcherService.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string? CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}