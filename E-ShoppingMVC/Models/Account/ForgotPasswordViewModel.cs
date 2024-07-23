using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email {  get; set; }
    }
}
