
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SufraMVC.Models.Customers;
using SufraMVC.Models.Restaurants;

namespace SufraMVC.Models.Orders
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [Required]
        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        // Navigation property for Restaurant
        public virtual Restaurant Restaurant { get; set; }
        public virtual Customer Customer { get; set; }


        // Collection of CartItems (private set for encapsulation)
        private readonly List<CartItem> _cartItems = new List<CartItem>();
        public virtual IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();



        // Business methods for manipulating cart items
        public void AddItem(CartItem cartItem)
        {
            _cartItems.Add(cartItem);
        }

        public IReadOnlyCollection<CartItem> GetCartItems()
        {
            return CartItems;
        }

        public void RemoveItem(CartItem cartItem)
        {
            _cartItems.Remove(cartItem);
        }
        public decimal CalculateTotal()
        {
            return _cartItems.Sum(item => item.Price * item.Quantity);
        }
    }
}
