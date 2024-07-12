using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models
{
	public class ColorModel
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage = "Vui lòng nhập tên màu")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Vui lòng nhập mã màu"), MaxLength(7, ErrorMessage = "Không quá 7 ký tự")]
		public string HexCode { get; set; }
	}
}
