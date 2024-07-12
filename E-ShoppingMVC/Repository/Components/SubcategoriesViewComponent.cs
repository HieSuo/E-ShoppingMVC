using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_ShoppingMVC.Repository.Components
{
	public class SubCategoriesViewComponent : ViewComponent
	{
		public readonly DataContext _dataContext;
		public SubCategoriesViewComponent(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public async Task<IViewComponentResult> InvokeAsync(int id)
		{
			return View(await _dataContext.Categories.Where(c=>c.ParentCategoryId == id).ToListAsync());
		}
	}
}
