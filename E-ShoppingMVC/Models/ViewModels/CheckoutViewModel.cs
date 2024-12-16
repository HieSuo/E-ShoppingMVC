namespace E_ShoppingMVC.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ShipAddressModel ShipAddressModel { get; set; }
        public ICollection<ShoppingCartItemModel> ShoppingCartItems{ get; set; }
        public string PaymentType { get; set; } // Thanh toán COD hoặc MOMO
        public string ShipMethod { get; set; } // Giao hàng tiết kiệm
    }
    public class OrderCreateModel
    {
        public string CustomerId { get; set; }
        public string CartId { get; set; }
        public int PaymentTypeId { get; set; }
        public int ShipMethodId { get; set; }
    }
}
