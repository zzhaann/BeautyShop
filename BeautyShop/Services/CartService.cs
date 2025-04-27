using System.Collections.ObjectModel;
using BeautyShop.Models;

namespace BeautyShop.Services
{
    public class CartService
    {
        private ObservableCollection<AppointmentCartItem> _cartItems = new();

        public ObservableCollection<AppointmentCartItem> GetCart() => _cartItems;

        public void AddToCart(AppointmentCartItem item)
        {
            _cartItems.Add(item);
        }

        public void ClearCart()
        {
            _cartItems.Clear();
        }

        public decimal GetTotal()
        {
            return _cartItems.Sum(x => x.Price);
        }
    }
}
