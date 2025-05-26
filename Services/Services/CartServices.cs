using Sufra.DTOs.CartDTOs;
using Sufra.DTOs.MenuDTOs;
using Sufra.Models.Customers;
using Sufra.Models.Orders;
using Sufra.Models.Restaurants;
using Sufra.Repositories.IRepositories;
using Sufra.Services.IServices;

namespace Sufra.Services.Services
{
    public class CartServices : ICartServices
    {

        private readonly ICustomerRepository _customerRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly ICartRepository _cartRepository;


        public CartServices(ICustomerRepository customerRepository, IMenuItemRepository menuItemRepository, ICartRepository cartRepository)
        {
            _customerRepository = customerRepository;
            _menuItemRepository = menuItemRepository;
            _cartRepository = cartRepository;
        }

        //----------------------------------------------------------------------------

        public async Task AddToCartAsync(AddToCartReqDTO addToCartReqDTO,int customerId)
        {
            Customer customer = await _customerRepository.GetByIdAsync(customerId);
            MenuItem menuItem = await _menuItemRepository.GetMenuItemByIdAsync(addToCartReqDTO.MenuItemId);

            if (customer == null || menuItem == null)
                throw new Exception("Customer or MenuItem not found");

            Cart customerCart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);

            if (customerCart == null)
            {
                customerCart = new Cart
                {
                    CustomerId = customer.Id,
                    RestaurantId = menuItem.RestaurantId,
                };
                await _cartRepository.CreateCartAsync(customerCart);
            }

            if (customerCart.RestaurantId != menuItem.RestaurantId)
            {
                throw new Exception("Can't add MenuItem from another Restaurant");
            }

            CartItem cartItem = new CartItem
            {
                CartId = customerCart.Id,
                MenuItemId = menuItem.Id,
                Price = menuItem.Price,
                Quantity = addToCartReqDTO.Quantity
            };

            customerCart.AddItem(cartItem);
            await _cartRepository.SaveAsync();
        }
        public async Task<IEnumerable<CartItemResponseDTO>> GetAllAsync(int customerId)
        {
            // Get the customer details
            Customer customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
                throw new Exception("Customer not found");

            // Get the cart for the customer
            Cart customerCart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);

            if (customerCart == null)
            {
                throw new Exception("Cart not found");
            }

            // Get the cart items from the cart
            var cartItems = customerCart.GetCartItems();

            // Map cart items to DTOs
            return cartItems.Select(item => new CartItemResponseDTO
            {
                CartItemId = item.Id,
                menuItemDTO = new MenuItemDTO
                {
                    RestaurantId = item.MenuItem.RestaurantId, 
                    MenuSectionId = item.MenuItem.MenuSectionId,
                    Name = item.MenuItem.Name,
                    MenuItemImg = item.MenuItem.MenuItemImg,
                    Description = item.MenuItem.Description,
                    Price = item.MenuItem.Price,
                    Availability = item.MenuItem.Availability
                },
                Quantity = item.Quantity,
                Price = item.Price
            });
        }
        public async Task ClearCart(int customerId)
        {
            Customer customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
                throw new Exception("Customer not found");

            Cart customerCart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);

            if (customerCart == null)
            {
                throw new Exception("Cart is Already Cleared");
            }

            await _cartRepository.DeleteCartAsync(customerCart);
        }
        public async Task RemoveFromCartAsync(int customerId, int cartItemId)
        {
            // Check if user exists
            Customer customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null)
                throw new Exception("Customer not found");

            // Get the cart for the customer
            Cart customerCart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);

            if (customerCart == null)
            {
                throw new Exception("Cart not found for the customer");
            }

            // Check if the cartItemId exists in the cart
            var cartItem = customerCart.CartItems.FirstOrDefault(item => item.Id == cartItemId);

            if (cartItem == null)
                throw new Exception("No cart item with this id found for this user");

            // Remove the item from the cart
            customerCart.RemoveItem(cartItem);  // Assuming you have a method like RemoveItem in the Cart class

            await _cartRepository.SaveAsync();  // Save the changes to the database
        }

    }
}
