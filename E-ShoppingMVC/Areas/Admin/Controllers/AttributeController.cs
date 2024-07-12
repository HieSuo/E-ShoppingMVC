using E_ShoppingMVC.Areas.Admin.Models.ViewModels;
using E_ShoppingMVC.Models;
using E_ShoppingMVC.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace E_ShoppingMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AttributeController : Controller
    {
        public readonly DataContext _dataContext;
        public AttributeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index()
        {
            SizeAndColorViewModel sizeAndColorViewModel = new SizeAndColorViewModel();
            sizeAndColorViewModel.Colors = await _dataContext.Colors.ToListAsync();
            sizeAndColorViewModel.Sizes = await _dataContext.Sizes.ToListAsync();
            return View(sizeAndColorViewModel);
        }
        [HttpGet]
        public IActionResult CreateNewColor()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewColor(ColorModel color)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Add(color);
                await _dataContext.SaveChangesAsync();
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
        public async Task<IActionResult> EditColor(int id)
        {
            ColorModel color = await _dataContext.Colors.FindAsync(id);
            return View(color);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditColor(ColorModel color)
        {
            ColorModel oldColor = _dataContext.Colors.Find(color.Id);
            if (ModelState.IsValid)
            {
                oldColor.Name = color.Name;
                oldColor.HexCode = color.HexCode;
                _dataContext.Colors.Update(oldColor);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Update color " + color.Name + " successfully";
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
        public async Task<IActionResult> DeleteColor(int id)
        {
            ColorModel color = await _dataContext.Colors.FindAsync(id);
            _dataContext.Colors.Remove(color);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Delete color " + color.Name + " successfully";
            return RedirectToAction("Index");
        }
        /*--------CRUD Size---------------*/
        [HttpGet]
        public IActionResult CreateSize()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSize(SizeModel size)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Add(size);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Create 1 size successfully";
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
        [HttpGet]
        public async Task<IActionResult> EditSize(int id)
        {
            SizeModel size = _dataContext.Sizes.Find(id);
            return View(size);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSize(SizeModel size)
        {
            SizeModel oldSize = _dataContext.Sizes.Find(size.Id);
            if (ModelState.IsValid)
            {
                oldSize.Name = size.Name;
                _dataContext.Sizes.Update(oldSize);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Update size succesfully";
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
        public async Task<IActionResult> DeleteSize(int id)
        {
            SizeModel size = await _dataContext.Sizes.FindAsync(id);
            _dataContext.Sizes.Remove(size);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Delete size " + size.Name + " successfully";
            return RedirectToAction("Index");
        }
    }
}
