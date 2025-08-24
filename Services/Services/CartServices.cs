using Sufra.DTOs.CartDTOs;
using Sufra.Exceptions;
using Sufra.Exceptions.Cart;
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
            if (customer == null) throw new NotFoundException<Customer>();

            MenuItem menuItem = await _menuItemRepository.GetMenuItemByIdAsync(addToCartReqDTO.MenuItemId);
            if (menuItem == null) throw new NotFoundException<MenuItem>();


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

            if (customerCart.RestaurantId != menuItem.RestaurantId) throw new CartRestaurantConflictException();

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

            if (customer == null) throw new NotFoundException<Customer>();

            Cart customerCart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);

            if (customerCart == null) throw new NotFoundException<Cart>();

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

            if (customer == null) throw new NotFoundException<Customer>();

            Cart customerCart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);

            if (customerCart == null) throw new NotFoundException<Cart>();
            if (customerCart.CartItems.Count == 0) throw new CartIsEmptyException();


            await _cartRepository.DeleteCartAsync(customerCart);
        }
        public async Task RemoveFromCartAsync(int customerId, int cartItemId)
        {
            Customer customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null) throw new NotFoundException<Customer>();

            Cart customerCart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);

            if (customerCart == null) throw new NotFoundException<Cart>();


            var cartItem = customerCart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);

            if (cartItem == null) throw new NotFoundException<CartItem>();

            customerCart.RemoveItem(cartItem);

            await _cartRepository.SaveAsync();
        }

    }
}
