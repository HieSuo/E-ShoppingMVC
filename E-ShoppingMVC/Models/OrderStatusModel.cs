using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models
{
	public class OrderStatusModel
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage ="Vui lòng nhập tên trạng thái.")]
		public string Name { get; set; }
		[Required(ErrorMessage ="Vui lòng nhập mã màu cho trạng thái")]
		public string Hexcode { get; set; }
	}
}
