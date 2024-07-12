using E_ShoppingMVC.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_ShoppingMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;
        public CategoryController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IActionResult Index()
        {
            var categories = _dataContext.Categories.Include(c=>c.SubCategories);
            return View();
        }
    }
}
