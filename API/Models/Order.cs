using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string OrderNumber { get; set; } = GenerateOrderNumber();

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Delivered, Cancelled

        [Required]
        public string PaymentStatus { get; set; } = "Pending"; // Pending, Paid, Failed

        public string? DeliveryAddress { get; set; }
        public string? CustomerNotes { get; set; }

        // Foreign keys
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser? User { get; set; }

        // Navigation properties
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        private static string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
        }
    }
}
