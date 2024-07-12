using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace E_ShoppingMVC.Models
{
	public class CategoryModel
	{
		[Key]
		public int Id { get; set; }
		[AllowNull]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "[Null]")]
        public int? ParentCategoryId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Slug { get; set; }

		public CategoryModel ParentCategory { get; set; }
		public ICollection<CategoryModel> SubCategories { get; set; }
	}
}
