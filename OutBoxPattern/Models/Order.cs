namespace OutBoxPattern.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}