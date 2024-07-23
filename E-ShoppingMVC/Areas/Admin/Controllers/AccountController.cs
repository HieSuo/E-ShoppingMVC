using E_ShoppingMVC.Areas.Admin.Models.ViewModels;
using E_ShoppingMVC.Models;
using E_ShoppingMVC.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E_ShoppingMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize]
    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<AppUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(DataContext dataContext, UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _dataContext.AppUsers.ToListAsync();           
            List<string> roles = new List<string>(); 
            foreach(var user in users)
            {
                IList<string> userRoles = await _userManager.GetRolesAsync(user);
                string strRole = "";
                foreach(var item in userRoles)
                {
                    strRole += item;
                }
                roles.Add(strRole);
            }

            UserRoleViewModel model = new UserRoleViewModel();
            model.appUserModels = users;
            model.ListRoles = roles;
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateUserViewModel model = new CreateUserViewModel();
            model.SelectedRoles = new List<SelectRoleViewModel>();
            var roles = await _roleManager.Roles.ToListAsync();
            foreach(var role in roles)
            {
                string roleid = role.Id;
                string roleName = role.Name;
                
                SelectRoleViewModel selectRoleViewModel = new SelectRoleViewModel();
                selectRoleViewModel.RoleId = roleid;
                selectRoleViewModel.RoleName = roleName;
                selectRoleViewModel.IsSelected = false;

                model.SelectedRoles.Add(selectRoleViewModel);
                
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid || model.Email == null || model.Password  == null || model.UserName ==null)
            {
                TempData["error"] = "Lỗi khi thêm mới người dùng.";
                return View(model);
            }
            if (ModelState.IsValid)
            {
                AppUserModel newUser = new AppUserModel
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };
                IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    foreach(var selectedRole in model.SelectedRoles)
                    {
                        if(selectedRole.IsSelected) 
                        {
                            string roleName = selectedRole.RoleName;
                            string roleId = selectedRole.RoleId;
                            await _userManager.AddToRoleAsync(newUser, selectedRole.RoleName);
                        }
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    TempData["error"] = "Error when create user ";
                }
            }
            //return roles to view
            model.SelectedRoles = new List<SelectRoleViewModel>();
            var roles = await _roleManager.Roles.ToListAsync();
            foreach (var role in roles)
            {
                string roleid = role.Id;
                string roleName = role.Name;

                SelectRoleViewModel selectRoleViewModel = new SelectRoleViewModel();
                selectRoleViewModel.RoleId = roleid;
                selectRoleViewModel.RoleName = roleName;
                selectRoleViewModel.IsSelected = false;

                model.SelectedRoles.Add(selectRoleViewModel);

            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id) 
        {
            EditUserViewModel model = new EditUserViewModel();
            var user = await _dataContext.AppUsers.FindAsync(id);
            model.UserModel = user;
            model.SelectedRoles = new List<SelectRoleViewModel>();
            var roles = await _roleManager.Roles.ToListAsync();
         
            foreach (var role in roles)
            {
                string roleid = role.Id;
                string roleName = role.Name;

                SelectRoleViewModel selectRoleViewModel = new SelectRoleViewModel();
                selectRoleViewModel.RoleId = roleid;
                selectRoleViewModel.RoleName = roleName;
                
                selectRoleViewModel.IsSelected = await _userManager.IsInRoleAsync(user,roleName);

                model.SelectedRoles.Add(selectRoleViewModel);

            }
        
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            AppUserModel oldUser = await _dataContext.Users.FindAsync(model.UserModel.Id);
            if (ModelState.IsValid)
            {

                oldUser.UserName = model.UserModel.UserName;
                oldUser.FirstName = model.UserModel.FirstName;
                oldUser.LastName = model.UserModel.LastName;
                oldUser.Email = model.UserModel.Email;
                oldUser.PhoneNumber = model.UserModel.PhoneNumber;

                IdentityResult result = await _userManager.UpdateAsync(oldUser);

                if (result.Succeeded)
                {
                    foreach (var selectedRole in model.SelectedRoles)
                    {
                        string roleName = selectedRole.RoleName;
                        string roleId = selectedRole.RoleId;
                        if (selectedRole.IsSelected)
                        {
                            await _userManager.AddToRoleAsync(oldUser, selectedRole.RoleName);
                        }
                        else
                        {
                            await _userManager.RemoveFromRoleAsync(oldUser, selectedRole.RoleName);
                        }
                    }
                    TempData["success"] = "Update account successfully";
                    return RedirectToAction("Index");
                }
                
            }

            model.SelectedRoles = new List<SelectRoleViewModel>();
            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var role in roles)
            {
                string roleid = role.Id;
                string roleName = role.Name;

                SelectRoleViewModel selectRoleViewModel = new SelectRoleViewModel();
                selectRoleViewModel.RoleId = roleid;
                selectRoleViewModel.RoleName = roleName;

                selectRoleViewModel.IsSelected = await _userManager.IsInRoleAsync(oldUser, roleName);

                model.SelectedRoles.Add(selectRoleViewModel);

            }
            return View(model);
        }
        public IActionResult AccessDenied(string returnUrl)
        {
            return Redirect(returnUrl);
        }
    }
}
