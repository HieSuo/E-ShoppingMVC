using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models
{
	public class AppUserModel : IdentityUser
	{
		[Required(ErrorMessage = "Vui lòng nhập tên")]
		public string FirstName { get; set; }
		[Required(ErrorMessage = "Vui lòng nhập họ")]
		public string LastName { get; set; }
		[Required(ErrorMessage = "Vui lòng nhập số điện thoại"),Phone(ErrorMessage ="Nhập số điện thoại") ]
		public string PhoneNumber {  get; set; }
	
	}
}
