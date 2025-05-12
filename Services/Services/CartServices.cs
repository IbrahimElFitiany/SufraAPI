

using DTOs;
using Services.IServices;
using Sufra_MVC.Models.CustomerModels;
using Sufra_MVC.Models.Orders;
using Sufra_MVC.Models.RestaurantModels;
using Sufra_MVC.Repositories;
using SufraMVC.Migrations;

namespace Sufra_MVC.Services.Services
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
    }
}
