using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_ShoppingMVC.Models
{
	public class ProductItemModel
	{
		[Key]
		public int Id { get; set; }
		public int ProductId {  get; set; }
		public string SKU { get; set; }
		public int SizeId { get; set; }
		public int ColorId { get; set; }
		[Required]
		[DataType(DataType.Currency)]
		[Column(TypeName = "money")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
		[Required]
		public int Quantity { get; set; }

		public ProductModel Product { get; set; }
		public ColorModel Color { get; set; }
		public SizeModel Size { get; set; }
	}
}
