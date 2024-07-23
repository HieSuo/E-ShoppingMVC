using E_ShoppingMVC.Models;
using E_ShoppingMVC.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_ShoppingMVC.Controllers
{
    public class ShippingAddressController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUserModel> _userManager;
        public ShippingAddressController(DataContext context, UserManager<AppUserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var listAddress = _context.ShipAddresses.Where(s=>s.UserId == user.Id).ToList();

            return View(listAddress);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ShipAddressModel shipAddressModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            shipAddressModel.UserId = user.Id;
            if (ModelState.IsValid)
            {
                _context.ShipAddresses.Add(shipAddressModel);
                await _context.SaveChangesAsync();
                TempData["success"] = "Create 1 shipAddressModel successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model has somethings error";
                List<String> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            return View();
        }
    }
}
