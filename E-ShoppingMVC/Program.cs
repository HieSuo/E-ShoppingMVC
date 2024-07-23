using E_ShoppingMVC.Models;
using E_ShoppingMVC.Repository;
using E_ShoppingMVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
    googleOptions.CallbackPath = "/dang-nhap-tu-google";
});
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(options =>
{
	options.UseSqlServer(configuration["ConnectionStrings:ConnectedDb"]);
});
//cofig identiy user
builder.Services.AddIdentity<AppUserModel, IdentityRole>().AddEntityFrameworkStores<DataContext>().AddRoles<IdentityRole>().AddDefaultTokenProviders(); ;
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = new PathString("/login/");
	options.AccessDeniedPath = new PathString("/khongduoctruycap.html");

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
                context.Response.Redirect("/login?ReturnUrl=" + Uri.EscapeDataString(context.Request.Path + context.Request.QueryString));
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
	options.Password.RequireDigit = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 1;
	options.Password.RequiredUniqueChars = 0;

	// Lockout settings.
	//options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	//options.Lockout.MaxFailedAccessAttempts = 5;
	//options.Lockout.AllowedForNewUsers = true;

	// User settings.
	options.User.AllowedUserNameCharacters =
	"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	options.User.RequireUniqueEmail = true;

    //options.SignIn.RequireConfirmedEmail = true;
    //options.SignIn.RequireConfirmedPhoneNumber = false;     //xac thuc so dien thoai
    //options.SignIn.RequireConfirmedAccount = true;
});
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/login/";
    options.LogoutPath = "/logout/";
    options.AccessDeniedPath = "/khongduoctruycap.html";
});
builder.Services.AddAuthentication("CookieAuth").AddCookie("CookieAuth", config =>
{
    config.LoginPath = "/login/";
    config.AccessDeniedPath = "/khongduoctruycap.html";

});
var mailsetting = configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailsetting);
builder.Services.AddSingleton<IEmailSender, SendMailService>();

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
