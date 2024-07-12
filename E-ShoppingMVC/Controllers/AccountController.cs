using E_ShoppingMVC.Models;
using E_ShoppingMVC.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_ShoppingMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly SignInManager<AppUserModel> _signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountController(DataContext dataContext, SignInManager<AppUserModel> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _dataContext = dataContext;
            _signInManager = signInManager;
            this.roleManager = roleManager;
        }
    
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
             await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
