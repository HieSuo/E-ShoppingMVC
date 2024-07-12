using E_ShoppingMVC.Models;

namespace E_ShoppingMVC.Areas.Admin.Models.ViewModels
{
    public class SizeAndColorViewModel
    {
        public SizeModel Size { get; set; }
        public ColorModel Color { get; set; }
        public ICollection<SizeModel> Sizes { get; set; }
        public ICollection<ColorModel> Colors { get; set; }

    }
}
