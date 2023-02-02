using System.ComponentModel.DataAnnotations;

namespace eShop.API.Entities.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string? Name { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 10)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }
        public byte[]? Image { get; set; }
        public int Quantity { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
