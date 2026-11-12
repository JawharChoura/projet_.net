using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        // Foreign keys
        public int CartId { get; set; }
        public virtual Cart? Cart { get; set; }

        public int ArticleId { get; set; }
        public virtual Article? Article { get; set; }
    }
}
