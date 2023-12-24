using System.ComponentModel.DataAnnotations;

namespace OrderMicroservice.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        public string? CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}