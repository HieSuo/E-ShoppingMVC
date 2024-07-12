using E_ShoppingMVC.Models;
using E_ShoppingMVC.Models.ViewModels;
using E_ShoppingMVC.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E_ShoppingMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        public readonly DataContext _dataContext;
        public CategoryController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["ParentSortParm"] = sortOrder == "parent" ? "parent_desc" : "parent";
            ViewData["IdSortParm"] = sortOrder == "id_sort" ? "id_desc" : "id_sort";

            if(searchString!= null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var categories = from s in _dataContext.Categories.Include(c=>c.ParentCategory) 
                             select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(c => c.Name.Contains(searchString) || c.ParentCategory.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    categories = categories.OrderByDescending(c => c.Name); 
                    break;
                case "parent_desc":
                    categories = categories.OrderByDescending(c => c.ParentCategoryId);
                    break;
                case "parent":
                    categories = categories.OrderBy(c => c.ParentCategoryId);
                    break;
                case "id_desc":
                    categories = categories.OrderByDescending(c => c.Id);
                    break;
                case "id_sort":
                    categories = categories.OrderBy(c => c.Id);
                    break;
                default:
                    categories = categories.OrderBy(c => c.Name); 
                    break;


            }

            int pageSize = 3;

            return View(await PaginatedList<CategoryModel>.CreateAsync(categories.AsNoTracking(), pageNumber??1, pageSize));
        }
        [HttpPost]
        public async Task<IActionResult> Index(string keyword, int he)
        {
            var categories = from p in _dataContext.Categories select p;

            if (!String.IsNullOrEmpty(keyword))
            {
                categories = categories.Where(c=>c.Name!.Contains(keyword));
            }
            TempData["keyword"]= keyword;
            return View(await categories.ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(c => c.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "categoryaName is exists on db");
                    TempData["error"] = "CategoryaName is exists on db";
                    return View(category);
                }
                _dataContext.Add(category);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Create 1 Category successfully";
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
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel category, int id)
        {
            CategoryModel oldCategory = _dataContext.Categories.Find(category.Id);
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(c => c.Slug == category.Slug);
                if (!oldCategory.Name.Equals(category.Name))
                {
                    if (slug != null)
                    {
                        ModelState.AddModelError("", "Category's Name is exists on db");
                        return View(category);
                    }
                }
                oldCategory.Name = category.Name;
                oldCategory.Slug = category.Slug;
                oldCategory.Description = category.Description;
                oldCategory.ParentCategoryId = category.ParentCategoryId;
                _dataContext.Update(oldCategory);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Update 1 Category successfully";
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
        }
        public async Task<IActionResult> Delete(int id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(id);
            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Da xoa danh muc " + category.Name;
            return Json(new {success = true});
        }
    }
}
