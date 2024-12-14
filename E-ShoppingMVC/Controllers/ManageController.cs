using E_ShoppingMVC.Models;
using E_ShoppingMVC.Models.Manage;
using E_ShoppingMVC.Repository;
using E_ShoppingMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_ShoppingMVC.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly SignInManager<AppUserModel> _signInManager;
        private readonly IEmailSender emailSender;
        private readonly ILogger<ManageController> _logger;
        private readonly DataContext _context;

        public ManageController(
            UserManager<AppUserModel> userManager, 
            SignInManager<AppUserModel> signInManager, 
            IEmailSender emailSender, 
            ILogger<ManageController> logger,
            DataContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.emailSender = emailSender;
            _logger = logger;
            _context = context;
        }
        [HttpGet]

        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewData["StatusMessage"]= 
                message == ManageMessageId.ChangePasswordSuccess ? "Đã thay đổi mật khẩu."
                : message == ManageMessageId.SetPasswordSuccess ? "Đã đặt lại mật khẩu."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "Có lỗi."
                : message == ManageMessageId.AddPhoneSuccess ? "Đã thêm số điện thoại."
                : message == ManageMessageId.RemovePhoneSuccess ? "Đã bỏ số điện thoại."
                : "";
            var user = await GetCurrentUserAsync();
            var shippingAdderess = await _context.ShipAddresses.Where(s => s.UserId == user.Id).ToListAsync(); 
            var model = new IndexViewModel
            {
                HasPassword = await _userManager.HasPasswordAsync(user),
                PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
                TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
                Logins = await _userManager.GetLoginsAsync(user),
                BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
                AuthenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user),
                profile = new EditExtraProfileViewModel()
                {
                    UserEmail = user.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                },
                ShipAddresses = shippingAdderess,
            };
            
            return View(model);
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }
        public Task<AppUserModel> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
