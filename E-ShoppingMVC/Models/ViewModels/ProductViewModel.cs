using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
}
