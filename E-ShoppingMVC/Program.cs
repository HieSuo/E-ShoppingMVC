using E_ShoppingMVC.Models;
using E_ShoppingMVC.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(options =>
{
	options.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectedDb"]);
});
//cofig identiy user
builder.Services.AddIdentity<AppUserModel, IdentityRole>().AddEntityFrameworkStores<DataContext>().AddRoles<IdentityRole>();
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = new PathString("/Admin/Login");
	options.AccessDeniedPath = new PathString("/Admin/Login/AccessDenied");

	options.Events = new CookieAuthenticationEvents {
        OnRedirectToLogin = context =>
        {
            var requestPath = context.Request.Path;
            if (requestPath.StartsWithSegments("/Admin"))
            {
                context.Response.Redirect("/Admin/Login/Index?ReturnUrl=" + Uri.EscapeDataString(context.Request.Path + context.Request.QueryString));
            }
            else
            {
                context.Response.Redirect("/Account/Login?ReturnUrl=" + Uri.EscapeDataString(context.Request.Path + context.Request.QueryString));
            }
            return Task.CompletedTask;
        },
        OnRedirectToAccessDenied = context =>
        {
            var requestPath = context.Request.Path;
            if (requestPath.StartsWithSegments("/Admin"))
            {
                context.Response.Redirect("/Admin/Account/AccessDenied");
            }
            else
            {
                context.Response.Redirect("/Account/AccessDenied");
            }
            return Task.CompletedTask;
        }
        
    };
});

builder.Services.AddRazorPages();
//account config
builder.Services.Configure<IdentityOptions>(options =>
{
	// Password settings.
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireUppercase = true;
	options.Password.RequiredLength = 6;
	options.Password.RequiredUniqueChars = 1;

	// Lockout settings.
	//options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	//options.Lockout.MaxFailedAccessAttempts = 5;
	//options.Lockout.AllowedForNewUsers = true;

	// User settings.
	options.User.AllowedUserNameCharacters =
	"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	options.User.RequireUniqueEmail = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
	name: "Areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}"
	);
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
