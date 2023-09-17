using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogApi.Models
{
    [Table("Produtos")]
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(80, ErrorMessage = "O nome deve ter no máximo {1} e no mínimo {2} caracteres",
            MinimumLength = 5)]
        public string? Name { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(8, 2)")]
        [Range(1, 10000, ErrorMessage = "O preço deve estar entre {1} e {2}")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 10)]
        public string? ImageUrl { get; set; }

        public float Stock { get; set; }
        public DateTime RegisterDate { get; set; }
        public Guid CategoryId { get; set; }

        [JsonIgnore]
        public Category? Category { get; set; }
    }
}
