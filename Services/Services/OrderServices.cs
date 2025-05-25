
using SufraMVC.DTOs;
using SufraMVC.Models.Customers;
using SufraMVC.Models.Orders;
using SufraMVC.Models.Restaurants;
using SufraMVC.Repositories.IRepositories;
using SufraMVC.Services.IServices;

namespace SufraMVC.Services.Services
{
    public class OrderServices:IOrderServices
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IRestaurantRepository _restaurantRepository;

        public OrderServices(IOrderRepository orderRepository, ICustomerRepository customerRepository, ICartRepository cartRepository, IRestaurantRepository restaurantRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _cartRepository = cartRepository;
            _restaurantRepository = restaurantRepository;
        }

        //-----------------------------------------

        public async Task CreateOrderAsync(OrderDTO orderDTO)
        {
            //check el awl 3ala el customer mawgod wala l2
            Customer customer = await _customerRepository.GetByIdAsync(orderDTO.CustomerId);
            if(customer == null)
            {
                throw new Exception("Customer Not Found");
            }

            //check 3la el cart hal feh cart ll user dah??
            Cart customerCart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);
            if(customerCart == null)
            {
                throw new Exception("no cart found for this user");
            }


            //e3ml order object
            Order newOrder = new Order
            {
                CustomerId = customer.Id,
                RestaurantId = customerCart.RestaurantId,
                OrderDate = orderDTO.OrderDate,
                Status = OrderStatus.Pending,
                TotalPrice = 0
            };

            await _orderRepository.CreateOrderAsync(newOrder);

            // for each cart item in customer's cart e3ml order item w assign it to el new order , and after that delete the cart
            foreach (CartItem cartItem in customerCart.CartItems)
            {
                OrderItem orderItem = new OrderItem
                {
                    OrderId = newOrder.Id,
                    MenuItemId = cartItem.MenuItemId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price
                };

                newOrder.AddOrderItem(orderItem);

                // Calculate the total price for the order
                newOrder.TotalPrice += orderItem.Quantity * orderItem.Price;
            }

            await _orderRepository.SaveAsync();

            await _cartRepository.DeleteCartAsync(customerCart);

            await _cartRepository.SaveAsync();

        }

        public async Task<OrderDTO> GetOrder(int orderId, int customerId)
        {
            //check el awl 3ala el customer mawgod wala l2
            Customer customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new Exception("Customer Not Found");
            }

            //check hal feh order wala l2 asln bl id dah
            Order order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order Not Found");
            }
            //check hal el order dah belongs to el customer dah
            if (order.CustomerId != customerId)
            {
                throw new Exception("Unauth req order doesn't belong to this customer");
            }

            //e3ml OrderDTO to send it to the Controller
            OrderDTO orderDTO = new OrderDTO
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                RestaurantId = order.RestaurantId,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Status = order.Status
            };


            return orderDTO;

        }
        public async Task<IEnumerable<OrderDTO>> GetRestaurantOrders(int restaurantId)
        {

            bool restaurantExists = await _restaurantRepository.ExistsAsync(restaurantId);
            if (!restaurantExists)
            {
                throw new KeyNotFoundException("Restaurant not found");
            }

            var orders = await _orderRepository.GetRestaurantOrders(restaurantId);

            return orders.Select(order => new OrderDTO
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                RestaurantId = order.RestaurantId,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate
            }).ToList();
        }
        public async Task<IEnumerable<OrderDTO>> GetCustomerOrders(int customerId)
        {

            var orders = await _orderRepository.GetCustomerOrders(customerId);

            return orders.Select(order => new OrderDTO
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                RestaurantId = order.RestaurantId,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate
            }).ToList();

        }
        public async Task<IEnumerable<OrderDTO>> GetAllOrders()
        {

            var orders = await _orderRepository.GetAllOrdersAsync();

            return orders.Select(order => new OrderDTO
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                RestaurantId = order.RestaurantId,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate
            }).ToList();

        }

        public async Task CancelOrderAsync(int orderId , int customerId)
        {
            //check el awl 3ala el customer mawgod wala l2
            Customer customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new Exception("Customer Not Found");
            }

            //check hal feh order wala l2 asln bl id dah
            Order order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order Not Found");
            }
            if (order.CustomerId != customerId)
            {
                throw new Exception("Unauth req order doesn't belong to this customer");
            }

            if (order.Status != OrderStatus.Pending)
            {
                throw new Exception("Order Can't be canceled rn");
            }

            await _orderRepository.CancelOrderAsync(order);
        }
        public async Task UpdateOrderStatus(OrderDTO orderDTO)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(orderDTO.RestaurantId);
            if (restaurant == null)
            {
                throw new Exception("Customer Not Found");
            }

            Order order = await _orderRepository.GetOrderByIdAsync(orderDTO.OrderId);
            if (order == null)
            {
                throw new Exception("Order Not Found");
            }

            if (order.RestaurantId != restaurant.Id)
            {
                throw new Exception("Unauth req order doesn't belong to this customer");
            }

            if(order.Status == OrderStatus.Canceled)
            {
                throw new Exception("order is canceled");
            }

            if (order.Status >= orderDTO.Status)
            {
                throw new InvalidOperationException(
                    $"Cannot change order status from {order.Status} to {orderDTO.Status}. " +
                    "Status can only progress forward");
            }

            order.Status = orderDTO.Status;

            await _orderRepository.SaveAsync();

        }
    }
}

