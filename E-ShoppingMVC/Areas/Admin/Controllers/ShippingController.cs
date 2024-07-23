using E_ShoppingMVC.Areas.Admin.Models.ViewModels;
using E_ShoppingMVC.Models;
using E_ShoppingMVC.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_ShoppingMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ShippingController : Controller
    {
        private readonly DataContext _context;
        public ShippingController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var shipMethods = await _context.ShipMethods.ToListAsync();
            var orderStatuses = await _context.OrderStatuses.ToListAsync();
            var bankProviders = await _context.BankProviders.ToListAsync();
            var paymentTypes = await _context.PaymentTypes.ToListAsync();
            var shippingManagerViewModel = new ShippingManagerViewModel
            {
                ShipMethods = shipMethods,
                OrderStatuses = orderStatuses,
                BankProviders = bankProviders,
                PaymentTypes = paymentTypes
            };               
            return View(shippingManagerViewModel);
        }
        [HttpPost] 
        public async Task<IActionResult> CreateShipMethod(ShipMethodModel shipMethod)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Model co van de r hehehe";
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
            _context.Add(shipMethod);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
