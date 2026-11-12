namespace Front.Models;
public class Article
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int CategoryId { get; set; }
    // Ajout de la propriété Category pour correspondre à l'API
    public Category? Category { get; set; }
}
