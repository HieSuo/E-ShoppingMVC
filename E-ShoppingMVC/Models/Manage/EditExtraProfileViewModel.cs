using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models.Manage
{
    public class EditExtraProfileViewModel
    {
        [Display(Name ="Tên tài khoản")]
        public string UserName { get; set; }
        [Display(Name ="Họ")]
        public string FirstName { get; set; }

        [Display(Name = "Tên")]
        public string LastName { get; set; }
        [Display(Name ="Địa chỉ Email")]
        public string UserEmail { get; set; }
        [Display(Name ="Số điện thoại")]
        public string PhoneNumber { get; set; }

    }
}
