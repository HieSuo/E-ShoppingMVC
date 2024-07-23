using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace E_ShoppingMVC.Models.ViewModels
{
    public class ProductViewModel
    {
        public ProductModel ProductModel { get; set; }
        public ICollection<ProductItemModel> ProductItems { get; set; }
        public decimal minPrice { get; set; }
    }
    public class ProductCardViewModel
    {
        public ProductModel productModel { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]

        public decimal minPrice { get; set; }
    }
    public class ProductDetailViewModel
    {
        public decimal minPrice { get; set; }
        public ProductModel Product { get; set; }
        public List<SizeModel> Sizes { get; set; }
        public List<ColorModel> Colors { get; set; }
        public List<ProductImageModel> Images { get; set; }
        public List<ProductModel> RecommendedProducts{ get; set; }
    }
}
