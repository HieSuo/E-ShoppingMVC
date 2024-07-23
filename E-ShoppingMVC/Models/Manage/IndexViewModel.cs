using Microsoft.AspNetCore.Identity;

namespace E_ShoppingMVC.Models.Manage
{
    public class IndexViewModel
    {
        public EditExtraProfileViewModel profile { get; set; }
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
        public string AuthenticatorKey {  get; set; }
    }
}
