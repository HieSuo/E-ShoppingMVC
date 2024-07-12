using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models
{
	public class PromotionModel
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage ="Vui lòng nhập tên chương trình.")]
		public string Name { get; set; }
		[Required(ErrorMessage ="Vui lòng nhập mô tả chương trình.")]
		public string Description { get; set; }
		[Required(ErrorMessage = "Vui lòng chọng ngày bắt đầu.")]
		public DateTime StartDate { get; set; }
		[Required(ErrorMessage = "Vui lòng chọng ngày kết thúc.")]
		public DateTime EndDate { get; set; }
		public string Slug;
	}
}
