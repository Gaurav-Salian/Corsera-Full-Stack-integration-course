namespace ServerApp.Models;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public int Stock { get; set; }
    public CategoryDto Category { get; set; } = null!;
}