using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models
{
	public class ShoppingCartModel
	{
		[Key]
		public int Id { get; set; }
		public string UserId { get; set; }
		public AppUserModel User { get; set; }
	}
}
