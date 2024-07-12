using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models
{
	public class PaymentTypeModel
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage ="Vui lòng nhập tên phương thức thanh toán.")]
		public string Name { get; set; }
	}
}
