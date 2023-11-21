using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogApi.Models;

[Table("Categorias")]
public class Category : BaseModel
{
    public Category()
    {
        Products = new Collection<Product>();
    }

    [Required]
    [StringLength(80)]
    public string? Name { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }

    public virtual ICollection<Product>? Products { get; set; }
}
