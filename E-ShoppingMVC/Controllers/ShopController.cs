using E_ShoppingMVC.Models.ViewModels;
using E_ShoppingMVC.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_ShoppingMVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly DataContext _dataContext;
        public ShopController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index(string sortOrder,string catFilter, int pg = 1 )
        {
            ViewData["CurrentSortOrder"] = sortOrder;
            ViewData["CurrentCategory"] = catFilter;
            var productList = await _dataContext.Products.ToListAsync();
            if (!String.IsNullOrEmpty(catFilter))
            {
                productList = await _dataContext.Products.Include(p=>p.Category).Where(p=>p.Category.Slug == catFilter).ToListAsync();
            }
            List<ProductCardViewModel> items = new List<ProductCardViewModel>();
            foreach(var product in productList)
            {
                var productItems = _dataContext.ProductItems.Where(p => p.ProductId == product.Id);
                if (productItems.Count()>0)
                {
                    ProductCardViewModel item = new ProductCardViewModel();
                    item.productModel = product;
                    decimal minPrice = productItems.Min(p => p.Price);        
                    item.minPrice = minPrice;
                    items.Add(item);
                }
                
            }

            if (!String.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "price_low_to_high":
                        items = items.OrderBy(p => p.minPrice).ToList();
                        break;
                    case "price_high_to_low":
                        items = items.OrderByDescending(p => p.minPrice).ToList();
                        break;
                }
            }

            const int pageSize = 12;
            if (pg < 1)
            {
                pg = 1;
            }
            int resCount = items.Count();
            var pager = new Pager(resCount, pg, pageSize);

            int recSkip = (pg-1) * pageSize;
            var data = items.Skip(recSkip).Take(pager.PageSize).ToList();
            int lastitem = pageSize*(pg-1)+data.Count;
            this.ViewBag.NumberOfList = new int[]{ recSkip+1, lastitem , resCount };
            this.ViewBag.Pager = pager;

            return View(data);
        }
        public async Task<IActionResult> ShopDetail(string slug)
        {
            return View();
        }
    }
}
