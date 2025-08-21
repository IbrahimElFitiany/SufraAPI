using Microsoft.EntityFrameworkCore;
using Sufra.Common.Enums;
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
        public async Task<(int Current, int Previous)> GetRestaurantOrdersTrendAsync(int restaurantId, TrendPeriod period)
        {
            DateTime now = DateTime.UtcNow;

            DateTime currentStart;
            DateTime previousStart;
            DateTime previousEnd;

            switch (period)
            {
                case TrendPeriod.Day:
                    currentStart = DateTime.SpecifyKind(now.Date, DateTimeKind.Utc);
                    previousStart = currentStart.AddDays(-1);
                    previousEnd = currentStart;
                    break;
                case TrendPeriod.Week:
                    int daysSinceWeekStart = (int)now.DayOfWeek;
                    currentStart = DateTime.SpecifyKind(now.Date.AddDays(-daysSinceWeekStart), DateTimeKind.Utc);
                    previousStart = currentStart.AddDays(-7);
                    previousEnd = currentStart;
                    break;
                case TrendPeriod.Month:
                    currentStart = DateTime.SpecifyKind(new DateTime(now.Year, now.Month, 1), DateTimeKind.Utc);
                    previousStart = currentStart.AddMonths(-1);
                    previousEnd = currentStart;
                    break;
                default:
                    throw new ArgumentException("Invalid period");
            }

            var result = await _context.Orders
                .Where(o => o.RestaurantId == restaurantId && (o.OrderDate >= previousStart && o.OrderDate <= now))
                .GroupBy(_ => 1) // group all relevant orders, then count to reduce DB hits
                .Select(g => new
                {
                    Current = g.Count(o => o.OrderDate >= currentStart),
                    Previous = g.Count(o => o.OrderDate >= previousStart && o.OrderDate < previousEnd)
                })
                .SingleOrDefaultAsync();

            return (result?.Current ?? 0, result?.Previous ?? 0);
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
