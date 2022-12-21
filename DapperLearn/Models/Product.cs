using System.ComponentModel.DataAnnotations;

namespace DapperLearn.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Product Name")]
        [StringLength(250, ErrorMessage = "Product Name must less or equals to 250 character")]
        [MinLength(2, ErrorMessage = "Product Name must grater then two character")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Please enter Product Price")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please enter Product Q")]
        public int Quantity { get; set; }
    }
}
