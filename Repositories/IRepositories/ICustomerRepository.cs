using Sufra.Models.Customers;

namespace Sufra.Repositories.IRepositories
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer customer);

        Task<Customer> GetCustomerByEmailAsync(string email);
        Task<Customer> GetCustomerByPhoneAsync(string phone);
        Task<Customer> GetByIdAsync(int customerId);

    }
}
