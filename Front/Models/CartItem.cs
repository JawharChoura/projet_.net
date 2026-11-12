namespace Front.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // IMPORTANT: L'API attend ArticleId, pas Article
        public int ArticleId { get; set; }

        // Pour l'affichage seulement
        public Article? Article { get; set; }
    }
}