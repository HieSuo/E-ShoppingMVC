using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models
{
	public class PromotionProductModel
	{
		[Key]
		public int Id { get; set; }
		public int PromotionId { get; set; }
		public int ProductId { get; set; }
		[Required(ErrorMessage ="Vui lòng nhập tỉ lệ giảm giá.")]
		public decimal DiscountRate { get; set; }
		public PromotionModel Promotion { get; set; }
		public ProductModel Product { get; set; }
	}
}
