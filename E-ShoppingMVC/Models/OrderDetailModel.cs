using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models
{
	public class OrderDetailModel
	{
		public int Id { get; set; }
		public int ProductItemId { get; set; }
		public int OrderId { get; set; }
		[DataType(DataType.Currency)]
		[Column(TypeName = "money")]
		public decimal Price { get; set; }
		public int Quantity { get; set; }

		public ProductItemModel ProductItem { get; set; }
		public OrderModel Order { get; set; }
	}
}
