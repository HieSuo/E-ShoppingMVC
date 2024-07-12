using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models
{
	public class UserPaymentModel
	{
		[Key]
		public int Id { get; set; }
		public string UserID { get; set; }
		public int PaymentTypeID { get; set; }
		public int ProviderID { get; set; }
		public string AccountNumber { get; set; }

		public AppUserModel User { get; set; }
		public PaymentTypeModel PaymentType { get; set; }
		public BankProviderModel BankProvider { get; set; }
	}
}
