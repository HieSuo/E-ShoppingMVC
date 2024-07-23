using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models.Account
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage ="Phải nhập {0}")]
		[EmailAddress(ErrorMessage ="Phải đúng định dạng email")]
		public string Email { get; set; }
		
		[Required(ErrorMessage = "Phải nhập {0}")]
		[StringLength( maximumLength:100,MinimumLength =3 ,ErrorMessage = "The {0} must be least {2} and at max {1} characters long.")]
		[DataType(DataType.Password)]
		[Display(Name = "Nhập mật khẩu mới")]
		public string Password { get; set; }
		[DataType(DataType.Password)]
		[Display(Name = "Nhập lại mật khẩu")]
		[Compare("Password", ErrorMessage ="The password and confirm password is not match")]
		public string ConfirmPassword {  get; set; }
		public string Code { get; set; }
	}
}
