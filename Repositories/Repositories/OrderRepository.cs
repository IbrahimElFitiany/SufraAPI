using Microsoft.EntityFrameworkCore;
using Sufra.Data;
using Sufra.DTOs.OrderDTOS;
using Sufra.Models.Orders;
using Sufra.Repositories.IRepositories;

namespace Sufra.Repositories.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly Sufra_DbContext _context;

        public OrderRepository(Sufra_DbContext sufra_DbContext)
        {
            _context = sufra_DbContext;
        }

        //------------------------

        public async Task CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
        public async Task CancelOrderAsync(Order order)
        {
            order.Status = OrderStatus.Canceled;
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        }
        public async Task<IEnumerable<Order>> GetRestaurantOrders(int restaurantId)
        {
            return await _context.Orders
                .Where(o => o.RestaurantId == restaurantId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<Order>> GetCustomerOrders(int customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .AsNoTracking()  
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> QueryOrdersAsync(OrderQueryDTO orderQueryDTO)
        {
            IQueryable<Order> orders = _context.Orders;

            if (orderQueryDTO.Status.HasValue) orders = orders.Where(o => o.Status == orderQueryDTO.Status);
            


            int skip = (orderQueryDTO.page - 1) * orderQueryDTO.pageSize;

            return await orders.Skip(skip).Take(orderQueryDTO.pageSize).ToListAsync();            
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
