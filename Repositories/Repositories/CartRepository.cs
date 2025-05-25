
using Microsoft.EntityFrameworkCore;
using SufraMVC.Data;
using SufraMVC.Models.Orders;
using SufraMVC.Repositories.IRepositories;

namespace SufraMVC.Repositories.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly Sufra_DbContext _context;

        public CartRepository(Sufra_DbContext sufra_DbContext)
        {
            _context = sufra_DbContext;
        }

        //------------------------

        public async Task<Cart> CreateCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
            return cart;
        }
        public async Task<Cart> GetCartByCustomerIdAsync(int customerId)
        {
            Cart customerCart = await _context.Carts.FirstOrDefaultAsync(cart => cart.CustomerId == customerId);
            return customerCart;
        }
        public async Task DeleteCartAsync(Cart cart)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
