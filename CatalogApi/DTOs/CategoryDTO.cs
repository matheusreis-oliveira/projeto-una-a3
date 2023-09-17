namespace CatalogApi.DTOs;

public class CategoryDTO
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public ICollection<ProductDTO>? Product { get; set; }
}
