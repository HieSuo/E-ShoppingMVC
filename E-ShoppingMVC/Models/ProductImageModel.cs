using E_ShoppingMVC.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_ShoppingMVC.Models
{
    public class ProductImageModel
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductImage {  get; set; }
        public ProductModel Product { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile ImageUpload { get; set; }
    }
}
