using E_ShoppingMVC.Models;
using E_ShoppingMVC.Models.Manage;
using E_ShoppingMVC.Models.ViewModels;
using E_ShoppingMVC.Repository;
using E_ShoppingMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace E_ShoppingMVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly SignInManager<AppUserModel> _signInManager;
        private readonly IEmailSender emailSender;
        private readonly ILogger<ManageController> _logger;
        private readonly DataContext _context;
        public CartController(
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

        public async Task<IActionResult> Index()
        {
            
            var user = await GetCurrentUserAsync();
            var userId = user.Id;
            if (userId == null)
            {
                return Unauthorized(); // Nếu người dùng chưa đăng nhập
            }
            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(c=>c.UserId == userId);
            List<ShoppingCartItemModel> cartItems = await _context.ShoppingCartItems.Include(c=>c.ProductItem).ThenInclude(p=>p.Product).Where(ci=>ci.CartId == cart.Id).ToListAsync();

            return View(cartItems);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int sizeId, int colorId, int quantity)
        {
            Console.WriteLine("Id: " + productId+"-"+sizeId +"-"+colorId+"-"+quantity);
            var productItem = await _context.ProductItems
                                .FirstOrDefaultAsync(pi => pi.ProductId == productId &&
                                   pi.SizeId == sizeId &&
                                   pi.ColorId == colorId);
            if (productItem == null)
            {
                TempData["ErrorMessage"] = "Phiên bản sản phẩm với màu sắc và kích cỡ này không tồn tại.";
                return RedirectToAction("Index", new { id = productId });
            }
            // Lấy hoặc tạo giỏ hàng của người dùng
            var user = await GetCurrentUserAsync();
            var userId = user.Id; // Hoặc lấy từ session, cookie
            var cart = await _context.ShoppingCarts
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new ShoppingCartModel { UserId = userId };
                _context.ShoppingCarts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
            var cartItem = await _context.ShoppingCartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cart.Id &&
                                           ci.ProductItemId == productItem.Id);

            if (cartItem == null)
            {
                // Nếu chưa có, thêm mới
                cartItem = new ShoppingCartItemModel
                {
                    CartId = cart.Id,
                    ProductItemId = productItem.Id,
                    Quantity = quantity
                };
                _context.ShoppingCartItems.Add(cartItem);
            }
            else
            {
                // Nếu đã có, cập nhật số lượng
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Sản phẩm đã được thêm vào giỏ hàng!";
            return RedirectToAction("Index", new { id = productId });
        }

        public async Task<IActionResult> DeleteCartItem(int id)
        {
            ShoppingCartItemModel cartItem = await _context.ShoppingCartItems.FindAsync(id);
            _context.ShoppingCartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            TempData["success"] = "Da xoa danh muc " + cartItem.Id;
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCartItems([FromBody] List<CartUpdateModel> cartUpdates)
        {
            if (cartUpdates == null || !cartUpdates.Any())
            {
                return Json(new { success = false, message = "No items to update!" });
            }

            decimal totalCartPrice = 0;

            foreach (var update in cartUpdates)
            {
                var cartItem = await _context.ShoppingCartItems.Include(c => c.ProductItem).FirstOrDefaultAsync(c => c.Id == update.Id);
                if (cartItem == null) continue;

                // Kiểm tra số lượng hợp lệ
                if (update.Quantity <= 0 || update.Quantity > cartItem.ProductItem.Quantity)
                {
                    return Json(new { success = false, message = $"Invalid quantity for item {cartItem.ProductItem.SKU}!" });
                }

                // Cập nhật số lượng
                cartItem.Quantity = update.Quantity;
            }

            // Lưu thay đổi và tính tổng giá giỏ hàng
            await _context.SaveChangesAsync();
            totalCartPrice = _context.ShoppingCartItems
                .Where(c => c.CartId == cartUpdates.First().CartId)
                .Sum(c => c.Quantity * c.ProductItem.Price);

            return Json(new { success = true,
                totalCartPrice = totalCartPrice.ToString("C0", CultureInfo.GetCultureInfo("vi-VN")) });
        }

        // Model cho dữ liệu cập nhật
        public class CartUpdateModel
        {
            public int Id { get; set; }
            public int Quantity { get; set; }
            public int CartId { get; set; } // ID giỏ hàng (nếu cần thiết)
        }
        public Task<AppUserModel> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
