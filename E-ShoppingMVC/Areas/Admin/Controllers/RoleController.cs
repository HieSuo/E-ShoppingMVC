using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_ShoppingMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(_roleManager.Roles);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            if(!_roleManager.RoleExistsAsync(role.Name).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole { Name = role.Name}).GetAwaiter().GetResult();
            }
            return RedirectToAction("Index");
        }
    }
}
