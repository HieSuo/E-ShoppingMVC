using E_ShoppingMVC.Models;

namespace E_ShoppingMVC.Areas.Admin.Models.ViewModels
{
    public class ProductDetailModelView
    {
        public ProductModel Product { get; set; }
        public ICollection<ProductItemModel> ProductItems { get; set; }
        public ICollection<ProductImageModel> ProductImages { get; set; }
    }
}
