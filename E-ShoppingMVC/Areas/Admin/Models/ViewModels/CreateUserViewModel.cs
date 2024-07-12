using E_ShoppingMVC.Models;

namespace E_ShoppingMVC.Areas.Admin.Models.ViewModels
{
    public class CreateUserViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public List<SelectRoleViewModel> SelectedRoles { get; set; }
    }
    public class SelectRoleViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
    public class EditUserViewModel
    {
        public AppUserModel UserModel { get; set; }
        public List<SelectRoleViewModel> SelectedRoles { get; set; }
    }
}
