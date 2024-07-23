using E_ShoppingMVC.Models;

namespace E_ShoppingMVC.Areas.Admin.Models.ViewModels
{
    public class ShippingManagerViewModel
    {
        public ICollection<ShipMethodModel> ShipMethods { get; set; }
        public ICollection<PaymentTypeModel> PaymentTypes { get; set; }
        public ICollection<BankProviderModel> BankProviders { get; set; }
        public ICollection<OrderStatusModel> OrderStatuses { get; set; }
    
    }
}
