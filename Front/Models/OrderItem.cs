namespace Front.Models;
public class OrderItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public Article Article { get; set; } = new();
}