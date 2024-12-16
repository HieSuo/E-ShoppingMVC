using E_ShoppingMVC.Models;
using E_ShoppingMVC.Models.ViewModels;
using E_ShoppingMVC.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_ShoppingMVC.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUserModel> _userManager;
        public CheckoutController(DataContext context, UserManager<AppUserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var shippingAddress = _context.ShipAddresses.Where(s=>s.UserId == user.Id).FirstOrDefault(); 
            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == user.Id);
            List<ShoppingCartItemModel> cartItems = await _context.ShoppingCartItems.Include(c => c.ProductItem).ThenInclude(p => p.Product).Where(ci => ci.CartId == cart.Id).ToListAsync();

            CheckoutViewModel checkoutViewModel = new CheckoutViewModel
            {
                FullName = user.FirstName + " " + user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ShipAddressModel = shippingAddress,
                ShoppingCartItems = cartItems
            };
            return View(checkoutViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> OrderJson(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!", errors = ModelState });
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var shippingAddress = _context.ShipAddresses.Where(s => s.UserId == user.Id).FirstOrDefault();
            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == user.Id);
            List<ShoppingCartItemModel> cartItems = await _context.ShoppingCartItems.Include(c => c.ProductItem).ThenInclude(p => p.Product).Where(ci => ci.CartId == cart.Id).ToListAsync();
            model.ShoppingCartItems = cartItems;
            // Trả về dữ liệu JSON để kiểm tra
            return Json(new
            {
                success = true,
                message = "Dữ liệu nhận được thành công!",
                data = new
                {
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Address = new
                    {
                        Province = model.ShipAddressModel.Province,
                        District = model.ShipAddressModel.District,
                        Ward = model.ShipAddressModel.Ward,
                        AddressDetail = model.ShipAddressModel.AddressDetail
                    },
                    PaymentType = model.PaymentType,
                    ShipMethod = model.ShipMethod,
                    ShoppingCartItems = model.ShoppingCartItems.Select(item => new
                    {
                        ProductName = item.ProductItem.Product.Name,
                        SKU = item.ProductItem.SKU,
                        Quantity = item.Quantity,
                        Price = item.ProductItem.Price,
                        Total = item.Quantity * item.ProductItem.Price
                    }),
                    SubTotal = model.ShoppingCartItems.Sum(p => p.ProductItem.Price * p.Quantity),
                    Total = model.ShoppingCartItems.Sum(p => p.ProductItem.Price * p.Quantity)
                }
            });
        }

    }
}
