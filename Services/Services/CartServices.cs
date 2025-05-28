using Sufra.DTOs.CartDTOs;
using Sufra.DTOs.MenuDTOs;
using Sufra.Exceptions;
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
            if (customer == null) throw new UserNotFoundException("Customer Not Found");

            MenuItem menuItem = await _menuItemRepository.GetMenuItemByIdAsync(addToCartReqDTO.MenuItemId);
            if (menuItem == null) throw new MenuItemNotFoundException("Menu Item Not Found");


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

            if (customerCart.RestaurantId != menuItem.RestaurantId) throw new CartRestaurantConflictException("Cart conflict: different restaurant");

            CartItem existingCartItem = customerCart.CartItems.FirstOrDefault(ci => ci.MenuItemId == menuItem.Id);


            if (existingCartItem != null)
            {
                existingCartItem.Quantity += addToCartReqDTO.Quantity;
            }

            else 
            {
                CartItem cartItem = new CartItem
                {
                    CartId = customerCart.Id,
                    MenuItemId = menuItem.Id,
                    Price = menuItem.Price,
                    Quantity = addToCartReqDTO.Quantity
                };

                customerCart.AddItem(cartItem);
            }

            await _cartRepository.SaveAsync();
        }
        public async Task<IEnumerable<CartListItemDTO>> GetAllAsync(int customerId)
        {
            Customer customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null) throw new UserNotFoundException("Customer not found");

            Cart customerCart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);

            if (customerCart == null) throw new CartNotFoundException("Cart not found");

            //N+1 query problem still figuring it out 
            var cartItems = customerCart.GetCartItems();

            // Map cart items to DTOs
            return cartItems.Select(item => new CartListItemDTO
            {
                CartItemId = item.Id,
                Name = item.MenuItem.Name,
                Description = item.MenuItem.Description,
                MenuItemImg = item.MenuItem.MenuItemImg,
                Quantity = item.Quantity,
                Price = item.Price
            });
        }
        public async Task ClearCart(int customerId)
        {
            Customer customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null) throw new UserNotFoundException("Customer not found");

            Cart customerCart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);

            if (customerCart == null) throw new CartNotFoundException("No Cart For This User");
            if (customerCart.CartItems.Count == 0) throw new CartIsEmptyException("Cart is Already Empty");


            await _cartRepository.DeleteCartAsync(customerCart);
        }
        public async Task RemoveFromCartAsync(int customerId, int cartItemId)
        {
            Customer customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null) throw new UserNotFoundException("Customer not found");

            Cart customerCart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);

            if (customerCart == null) throw new CartNotFoundException("Cart not found for the customer");


            var cartItem = customerCart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);

            if (cartItem == null) throw new CartItemNotFoundException("No cart item with this id found for this user");

            customerCart.RemoveItem(cartItem);

            await _cartRepository.SaveAsync();
        }

    }
}
