using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models
{
	public class ShipMethodModel
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage ="Vui lòng nhập tên phương thức giao hàng.")]
		public string Name { get; set; }
		public decimal Price { get; set; }
	}
}
