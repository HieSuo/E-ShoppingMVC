using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models
{
	public class BankProviderModel
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
	}
}
