namespace E_ShoppingMVC.Models
{
	public class ShoppingCartItemModel
	{
		public int Id { get; set; }
		public int CartId { get; set; }
		public int ProductItemId { get; set; }
		public int Quantity { get; set; }

		public ShoppingCartModel Cart { get; set; }
		public ProductItemModel ProductItem { get; set; }
	}
}
