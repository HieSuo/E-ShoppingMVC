using E_ShoppingMVC.Models;

namespace E_ShoppingMVC.Areas.Admin.Models.ViewModels
{
    public class UserRoleViewModel
    {
        public ICollection<AppUserModel> appUserModels { get; set; }
        public List<string> ListRoles { get; set; }
    }
}
