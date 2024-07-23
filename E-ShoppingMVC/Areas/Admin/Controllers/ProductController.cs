using E_ShoppingMVC.Areas.Admin.Models.ViewModels;
using E_ShoppingMVC.Models;
using E_ShoppingMVC.Models.ViewModels;
using E_ShoppingMVC.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace E_ShoppingMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(DataContext dataContext, IWebHostEnvironment webHostEnvironment) {
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter,int categoryFilter,int currentCategory, int pg = 1)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParam"] = sortOrder =="id_sort" ? "id_desc" : "id_sort";

            if (searchString != null)
            {
                pg = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            if (categoryFilter > 0)
            {
                pg = 1;
            }
            else
            {
                categoryFilter = currentCategory;
            }
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentCategory"] = categoryFilter;

            var products = from p in _dataContext.Products.Include(p => p.Category)
                             select p;

            if (categoryFilter > 0)
            {
                products = products.Where(p => p.CategoryId == categoryFilter);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "id_desc":
                    products = products.OrderByDescending(p => p.Id);
                    break;
                case "id_sort":
                    products = products.OrderBy(p => p.Id);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }


            const int pageSize = 10;
            if (pg < 1)
            {
                pg = 1;
            }

            int resCount = products.Count();
            var pager = new Pager(resCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = products.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;
            ViewBag.Categories = _dataContext.Categories;

            //return View( await _dataContext.Products.OrderBy(p=>p.Id).Include(p=>p.Category).ToListAsync());
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-");
                var slug = _dataContext.Products.FirstOrDefault(p=>p.Slug==product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Tên sản phẩm đã tồn tại trên hệ thống.");
                    return View(product);        
                }
                if(product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "-" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);

                    await product.ImageUpload.CopyToAsync(fs);

                    fs.Close();
                    product.Image = imageName;
                }
                else
                {
                    TempData["error"] = "Image is null";
                    return View(product);
                }

                _dataContext.Add(product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Create 1 product successfully";
                return RedirectToAction("Index");
            }
            else
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

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductModel product)
        {
            ProductModel oldProduct = _dataContext.Products.Find(product.Id);
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-");
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                if(!oldProduct.Slug.Equals(product.Slug) && slug != null)
                {
                    ModelState.AddModelError("", "Product'name existed.");
                    return View(product);
                }
                if(product.ImageUpload != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");

                    string oldFileImage = Path.Combine(uploadDir, oldProduct.Image);
                    if (System.IO.File.Exists(oldFileImage))
                    {
                        System.IO.File.Delete(oldFileImage);
                    }
                    string imageName = Guid.NewGuid().ToString()+"-"+product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);

                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    oldProduct.Image = imageName;
                }

                oldProduct.Name = product.Name;
                oldProduct.Price = product.Price;
                oldProduct.CategoryId = product.CategoryId;
                oldProduct.Description = product.Description;
                oldProduct.Slug = product.Slug;

                _dataContext.Products.Update(oldProduct);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Update product " + oldProduct.Name + " successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Form has exceptions";
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
        public async Task<IActionResult> Delete(int id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(id);

            //Remove image
            string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");

            string oldFileImage = Path.Combine(uploadDir, product.Image);
            if (System.IO.File.Exists(oldFileImage))
            {
                System.IO.File.Delete(oldFileImage);
            }


            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Delete 1 prduct successfully.";
            if (product == null)
            {
                TempData["error"] = "Null product to delete";
            }
            return Json(new {success = true});
        }
        public async Task<IActionResult> Detail(int id)
        {
            ProductDetailModelView productDetail = new ProductDetailModelView();
            productDetail.Product = await _dataContext.Products.FindAsync(id);
            productDetail.Product.Category = _dataContext.Categories.Find(productDetail.Product.CategoryId);

            productDetail.ProductItems = await _dataContext.ProductItems.Where(p=>p.ProductId==id).Include(p=>p.Color).Include(p=>p.Size).OrderBy(p=>p.ColorId).ToListAsync();
            productDetail.ProductImages = await _dataContext.ProductImages.Where(i => i.ProductId == id).ToListAsync();                                                                                                         
            return View(productDetail);
        }
        [HttpGet]
        public async Task<IActionResult> CreateProductItem(int id)
        {
            ViewBag.Colors = new SelectList(_dataContext.Colors, "Id", "Name");
            ViewBag.Sizes = new SelectList(_dataContext.Sizes, "Id", "Name");
            CreateProductItemViewModel productItemVM = new CreateProductItemViewModel();
            productItemVM.Product = await _dataContext.Products.FindAsync(id);
            return View(productItemVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductItem(CreateProductItemViewModel productItemVM)
        {
            ProductItemModel prdItem = productItemVM.ProductItem;
            productItemVM.Product = await _dataContext.Products.FindAsync(prdItem.ProductId);
            ColorModel color = _dataContext.Colors.Find(prdItem.ColorId);
            SizeModel size = _dataContext.Sizes.Find(prdItem.SizeId);
            if (ModelState.IsValid)
            {
                prdItem.SKU = prdItem.ProductId +"-"+ color.Name +"-"+ size.Name;
                var skuExist = await _dataContext.ProductItems.FirstOrDefaultAsync(p => p.SKU == prdItem.SKU);
                if (skuExist != null)
                {
                    ModelState.AddModelError("", "SKU was exist");
                    TempData["error"] = "SKU was exist";
                    return View(productItemVM);
                }
                _dataContext.Add(prdItem);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Add Product item successfully";
                return RedirectToAction("Detail", new {id= productItemVM.ProductItem.ProductId});

            }
            else
            {
                TempData["error"] = "Form has exceptions";
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
        public async Task<IActionResult> DeleteProductItem(int id)
        {
            ProductItemModel productItem = await _dataContext.ProductItems.FindAsync(id);
            _dataContext.ProductItems.Remove(productItem);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Delete 1 product item successfully.";
            if (productItem == null)
            {
                TempData["error"] = "Null product to delete";
            }
            return RedirectToAction("Detail", new { id = productItem.ProductId });
        }
        [HttpGet]
        public async Task<IActionResult> EditProductItem(int id)
        {
            ViewBag.Colors = new SelectList(_dataContext.Colors, "Id", "Name");
            ViewBag.Sizes = new SelectList(_dataContext.Sizes, "Id", "Name");
            ProductItemModel productItem = await _dataContext.ProductItems.Include(p=>p.Product).FirstOrDefaultAsync(p=>p.Id==id);
            return View(productItem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductItem(ProductItemModel prdItem)
        {
            ViewBag.Colors = new SelectList(_dataContext.Colors, "Id", "Name");
            ViewBag.Sizes = new SelectList(_dataContext.Sizes, "Id", "Name");
            ProductItemModel oldVersion = await _dataContext.ProductItems.FindAsync(prdItem.Id);
            ColorModel color = _dataContext.Colors.Find(prdItem.ColorId);
            SizeModel size = _dataContext.Sizes.Find(prdItem.SizeId);
            prdItem.Product = await _dataContext.Products.FindAsync(prdItem.ProductId);
            if (ModelState.IsValid)
            {
               prdItem.SKU = prdItem.ProductId + "-" + color.Name + "-" + size.Name;
               var skuExist = await _dataContext.ProductItems.FirstOrDefaultAsync(p => p.SKU == prdItem.SKU);
               if(prdItem.SKU != oldVersion.SKU && skuExist != null)
               {
                   ModelState.AddModelError("", "SKU was exist");
                   TempData["error"] = "SKU was exist";
                   return View(prdItem);
               }
               oldVersion.SKU = prdItem.SKU;
               oldVersion.Price = prdItem.Price;
               oldVersion.SizeId = prdItem.SizeId;
               oldVersion.ColorId = prdItem.ColorId;
               _dataContext.ProductItems.Update(oldVersion);
               await _dataContext.SaveChangesAsync();
               TempData["success"] = "Product item was update successfully";
               return RedirectToAction("Detail", new { id = prdItem.ProductId });

            }
            else
            {
               TempData["error"] = "Form has exceptions";
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
        public async Task<IActionResult> AddImage(IFormFile image, int productId)
        {

            ProductImageModel model = new ProductImageModel();
            model.ProductId = productId;
            if (image != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                string imageName = Guid.NewGuid().ToString() + "-" + image.FileName;
                string filePath = Path.Combine(uploadDir, imageName);

                FileStream fs = new FileStream(filePath, FileMode.Create);

                await image.CopyToAsync(fs);

                fs.Close();
                model.ProductImage = imageName;
            }
            else
            {
                TempData["error"] = "Image is null";
                return Json(new { success = false });
            }

            _dataContext.ProductImages.Add(model);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Add 1 product image successfully";
            return Json(new { success = true });
        }
        public async Task<IActionResult> DeleteProductImage(int id)
        {
            ProductImageModel model = await _dataContext.ProductImages.FindAsync(id);
            string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");

            string oldFileImage = Path.Combine(uploadDir, model.ProductImage);
            if (System.IO.File.Exists(oldFileImage))
            {
                System.IO.File.Delete(oldFileImage);
            }
            _dataContext.ProductImages.Remove(model);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Delete 1 product image item successfully.";
            if (model == null)
            {
                TempData["error"] = "No image to delete";
            }
            return RedirectToAction("Detail", new { id = model.ProductId });
        }


    }
}
