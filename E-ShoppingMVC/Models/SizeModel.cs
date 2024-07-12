using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models
{
	public class SizeModel
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage ="Vui lòng nhập tên kích cỡ")]
		public string Name { get; set; }
	
	}
}
