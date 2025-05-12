using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Sufra_MVC.Data;
using Sufra_MVC.Models.CustomerModels;

namespace Sufra_MVC.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly Sufra_DbContext _context;

        public CustomerRepository(Sufra_DbContext sufra_DbContext)
        {
            _context = sufra_DbContext;
        }

        //------------------------------------------------
        public async Task AddCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);

        }
        public async Task<Customer> GetCustomerByPhoneAsync(string phone)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Phone == phone);
        }
        public async Task<Customer> GetByIdAsync(int customerId)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
        }


    }
}
