using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_ShoppingMVC.Models
{
	public class OrderModel
	{
		[Key]
		public int Id { get; set; }
		public string UserId { get; set; }
		[DataType(DataType.DateTime)]
		public DateTime OrderDate { get; set; }
		public int UserPaymentId { get; set; }
		public int ShipAddressId { get; set; }
		public int ShipMethodId { get; set; }
		[DataType(DataType.Currency)]
		[Column(TypeName = "money")]
		public decimal Total { get; set; }
		public int OrderStatusId { get; set; }

		public AppUserModel User { get; set; }
		public UserPaymentModel UserPayment { get; set; }
		public ShipAddressModel Address { get; set; }
		public ShipMethodModel ShipMethod { get; set; }
		public OrderStatusModel OrderStatus { get; set; }
	}
}
