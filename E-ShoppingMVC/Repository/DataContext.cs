using E_ShoppingMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_ShoppingMVC.Repository
{
	public class DataContext : IdentityDbContext<AppUserModel>
	{
		public DataContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<AppUserModel> AppUsers { get; set; }
		public DbSet<IdentityRole> Roles { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<ProductItemModel> ProductItems { get; set; }
		public DbSet<ColorModel> Colors { get; set; }
		public DbSet<SizeModel> Sizes { get; set; }
		public DbSet<OrderStatusModel> OrderStatuses { get; set; }
		public DbSet<ShipMethodModel> ShipMethods { get; set; }
		public DbSet<PaymentTypeModel> PaymentTypes { get; set; }
		public DbSet<UserPaymentModel> UserPayments { get; set; }
		public DbSet<BankProviderModel> BankProviders { get; set; }
		public DbSet<ShoppingCartModel> ShoppingCarts { get; set; }
		public DbSet<ShoppingCartItemModel> ShoppingCartItems { get; set; }
		public DbSet<ShipAddressModel> ShipAddresses { get; set; }
		public DbSet<OrderModel> Orders { get; set; }
		public DbSet<OrderDetailModel> OrderDetails {get; set;}
		public DbSet<PromotionModel> Promotions { get; set; }
		public DbSet<PromotionProductModel> PromotionProducts { get; set; }
		public DbSet<UserReviewModel> UserReviews { get; set; }

		public DbSet<ProductImageModel> ProductImages { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CategoryModel>()
				.HasOne(c => c.ParentCategory)
				.WithMany(c => c.SubCategories)
				.HasForeignKey(c => c.ParentCategoryId)
				.OnDelete(DeleteBehavior.SetNull);

			base.OnModelCreating(modelBuilder);


		}

	}
}
