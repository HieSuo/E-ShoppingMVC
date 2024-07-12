using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_ShoppingMVC.Repository.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        public readonly DataContext _dataContext;
        public CategoriesViewComponent(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IViewComponentResult> InvokeAsync() { 
            
            return View(await _dataContext.Categories.Where(c=>c.ParentCategoryId == null).ToListAsync());
        }
    }
}
