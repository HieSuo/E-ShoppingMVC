using System.ComponentModel.DataAnnotations;

namespace E_ShoppingMVC.Models
{
	public class UserReviewModel
	{
		[Key]
		public int Id { get; set; }
		public string UserId { get; set; }
		public int OrderDetailId { get; set; }
		[Required]
		public decimal RatingValue { get; set; }
		public string Comment { get; set; }
		public AppUserModel UserModel { get; set; }
		public OrderDetailModel OrderDetail { get; set; }
	}

}
