using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models
{
	public class ShipAddressModel
	{
		[Key]
		public int Id { get; set; }
		public string UserId { get; set; }
		[Required]
		public string Province { get; set; }
		[Required]
		public string District { get; set; }
		[Required]
		public string Ward { get; set; }
		[Required]
		public string AddressDetail { get; set; }
		
		public AppUserModel User { get; set; }
	}
}
