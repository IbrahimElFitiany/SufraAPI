using Microsoft.EntityFrameworkCore;
using Sufra.Data;
using Sufra.DTOs.OrderDTOS;
using Sufra.Models.Orders;
using Sufra.Models.Restaurants;
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
        public async Task<Order> GetOrderDetailedByIdAsync(int orderId)
        {
            return await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.MenuItem).FirstOrDefaultAsync(o => o.Id == orderId);
        }
        public async Task<IEnumerable<Order>> GetRestaurantOrders(int restaurantId , OrderQueryDTO orderQuery)
        {
            IQueryable<Order> orders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Restaurant)
                .Where(o => o.RestaurantId == restaurantId).AsNoTracking();

            if (orderQuery.Status.HasValue) orders = orders.Where(o => o.Status == orderQuery.Status);

            int skip = (orderQuery.page - 1) * orderQuery.pageSize;

            return await orders
                .OrderBy(r => r.Id)
                .Skip(skip)
                .Take(orderQuery.pageSize)
                .ToListAsync();
        }
        public async Task<IEnumerable<Order>> GetCustomerOrders(int customerId , OrderQueryDTO orderQuery)
        {
            IQueryable<Order> orders = _context.Orders.Include(o => o.Restaurant).Where(o => o.CustomerId == customerId).AsNoTracking();

            if (orderQuery.Status.HasValue) orders = orders.Where(o => o.Status == orderQuery.Status);

            int skip = (orderQuery.page - 1) * orderQuery.pageSize;
            return await orders.Skip(skip).Take(orderQuery.pageSize).ToListAsync();
        }
        public async Task<IEnumerable<Order>> QueryOrdersAsync(OrderQueryDTO orderQueryDTO)
        {
            IQueryable<Order> orders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Restaurant);

            if (orderQueryDTO.Status.HasValue) orders = orders.Where(o => o.Status == orderQueryDTO.Status);
            


            int skip = (orderQueryDTO.page - 1) * orderQueryDTO.pageSize;

            return await orders
                .OrderBy(r => r.Id)
                .Skip(skip)
                .Take(orderQueryDTO.pageSize)
                .ToListAsync();            
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
