using E_ShoppingMVC.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_ShoppingMVC.Models
{
	public class ProductModel
	{
		[Key]
		public int Id { get; set; }
		public int CategoryId { get; set; }
		[Required(ErrorMessage ="Vui lòng nhập tên sản phẩm.")]
		public string Name { get; set; }
		public string Slug { get; set; }
		[Required(ErrorMessage ="Vui lòng nhập mô tả sản phẩm.")]
		public string Description { get; set; }
		public string Image { get; set; }

		public CategoryModel Category { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
		[Required(ErrorMessage ="Vui lòng nhập giá sản phẩm")]
		public decimal Price { get; set; }
		
    }
}
