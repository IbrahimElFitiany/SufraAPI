using Microsoft.EntityFrameworkCore;
using SufraMVC.Data;
using SufraMVC.Models.Orders;
using SufraMVC.Repositories.IRepositories;

namespace SufraMVC.Repositories.Repositories
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
                .AsNoTracking()  // Better performance for read-only
                .ToListAsync();
        }
        public async Task<IEnumerable<Order>> GetCustomerOrders(int customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .AsNoTracking()  
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()   //For Admin
        {
            return await _context.Orders.ToListAsync();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
