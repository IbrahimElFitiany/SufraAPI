using Microsoft.EntityFrameworkCore;
using Models.Orders;
using Sufra_MVC.Data;
using Sufra_MVC.Models.CustomerModels;
using Sufra_MVC.Models.Orders;
using Sufra_MVC.Models.RestaurantModels;
using Sufra_MVC.Repositories.IRepositories;

namespace Sufra_MVC.Repositories
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
        }
        public async Task<Cart> GetCartByCustomer(Customer customer)
        {
            Cart customerCart = await _context.Carts.FirstOrDefaultAsync(cart => cart.CustomerId == customer.Id);
            return customerCart;
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
