using MailKit.Search;
using Sufra.DTOs.OrderDTOS;
using Sufra.Exceptions;
using Sufra.Models.Customers;
using Sufra.Models.Orders;
using Sufra.Models.Restaurants;
using Sufra.Repositories.IRepositories;
using Sufra.Services.IServices;

namespace Sufra.Services.Services
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
            Customer customer = await _customerRepository.GetByIdAsync(orderDTO.CustomerId);

            if(customer == null) throw new UserNotFoundException("Customer Not Found");

            Cart customerCart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);

            if(customerCart == null) throw new CartNotFoundException("no cart found for this user");
            if (customerCart.CartItems.Count == 0) throw new CartIsEmptyException("Can't Place Order Cart is Empty");

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

                newOrder.TotalPrice += orderItem.Quantity * orderItem.Price;
            }

            await _orderRepository.SaveAsync();

            await _cartRepository.DeleteCartAsync(customerCart);

            await _cartRepository.SaveAsync();
        }

        public async Task<OrderDetailedDTO> GetOrderAsync(int orderId, int customerId)
        {
            Customer customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null) throw new UserNotFoundException("Customer Not Found");


            Order order = await _orderRepository.GetOrderDetailedByIdAsync(orderId);

            if (order == null) throw new OrderNotFoundException("Order Not Found");

            if (order.CustomerId != customerId) throw new UnauthorizedAccessException("Order doesn't belong to this customer");


            OrderDetailedDTO orderDetailedDTO = new OrderDetailedDTO
            {
                Id = order.Id,
                RestaurantId = order.RestaurantId,
                RestaurantName = order.Restaurant.Name,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                OrderItems = order.OrderItems.Select(item => new OrderItemDTO
                {
                    Id = item.Id,
                    OrderId = item.Id,
                    MenuItemId = item.MenuItemId,
                    MenuItemName = item.MenuItem.Name,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            return orderDetailedDTO;
        }
        public async Task<IEnumerable<OrderItemListDTO>> GetCustomerOrdersAsync(int customerId , OrderQueryDTO orderQuery)
        {
            Customer customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null) throw new UserNotFoundException("UserNotFound");

            var orders = await _orderRepository.GetCustomerOrders(customerId ,orderQuery);

            return orders.Select(order => new OrderItemListDTO
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                RestaurantName = order.Restaurant.Name,
                RestaurantImageUrl = order.Restaurant.ImgUrl
            }).ToList();
        }
        public async Task<IEnumerable<OrderDTO>> GetRestaurantOrdersAsync(int restaurantId , OrderQueryDTO orderQuery)
        {

            bool restaurantExists = await _restaurantRepository.ExistsAsync(restaurantId);
            if (!restaurantExists)
            {
                throw new RestaurantNotFoundException("Restaurant not found");
            }

            var orders = await _orderRepository.GetRestaurantOrders(restaurantId , orderQuery);

            return orders.Select(order => new OrderDTO //maybe I will create a customDTO
            {
                OrderId = order.Id,

                CustomerId = order.CustomerId,
                CustomerName = order.Customer.Fname,
                CustomerEmail = order.Customer.Email,
                CustomerPhone = order.Customer.Phone,

                RestaurantId = order.RestaurantId,
                RestaurantName = order.Restaurant.Name,
               
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate
            }).ToList();
        }
        public async Task<IEnumerable<OrderDTO>> QueryOrdersAsync(OrderQueryDTO orderQueryDTO)
        {
            var orders = await _orderRepository.QueryOrdersAsync(orderQueryDTO);

            return orders.Select(order => new OrderDTO
            {
                OrderId = order.Id,

                CustomerId = order.CustomerId,
                CustomerName = order.Customer.Fname,
                CustomerEmail = order.Customer.Email,
                CustomerPhone = order.Customer.Phone,

                RestaurantId = order.RestaurantId,
                RestaurantName = order.Restaurant.Name,

                Status = order.Status,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate
            }).ToList();

        }


        public async Task CancelOrderAsync(int orderId , int customerId)
        {
            Customer customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer == null) throw new UserNotFoundException("Customer Not Found");

            Order order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order == null) throw new OrderNotFoundException("Order Not Found");

            if (order.CustomerId != customerId) throw new UnauthorizedAccessException("UnAuthorized");

            if (order.Status != OrderStatus.Pending)
            {
                throw new OrderCancellationException("Order cannot be canceled unless it is in the Pending state.");
            }

            await _orderRepository.CancelOrderAsync(order);
        }
        public async Task UpdateOrderStatus(OrderDTO orderDTO)
        {
            Restaurant restaurant = await _restaurantRepository.GetByIdAsync(orderDTO.RestaurantId);

            if (restaurant == null) throw new RestaurantNotFoundException("Restaurant Not Found");
            if (!restaurant.IsApproved) throw new RestaurantNotApprovedException("Restaurant Not Approved");


            Order order = await _orderRepository.GetOrderByIdAsync(orderDTO.OrderId);

            if (order == null)  throw new OrderNotFoundException("Order Not Found");
            if (order.RestaurantId != restaurant.Id) throw new OrderUnauthorizedAccessException("UnAuthoriezed Order doesn't Belong To Restaurant");


            if(order.Status == OrderStatus.Canceled) throw new OrderIsAlreadyCanceledException($"Order with ID {order.Id} is already canceled.");


            if (order.Status >= orderDTO.Status)
            {
                throw new InvalidOrderStatusTransitionException( $"Cannot change order status from {order.Status.ToString()} to {orderDTO.Status.ToString()}. " + "Status can only progress forward");
            }


            order.Status = orderDTO.Status;
            await _orderRepository.UpdateOrderAsync(order);

        }
    }
}

