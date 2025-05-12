using Sufra_MVC.Models.CustomerModels;

namespace Sufra_MVC.Repositories
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer customer);

        Task<Customer> GetCustomerByEmailAsync(string email);
        Task<Customer> GetCustomerByPhoneAsync(string phone);
        Task<Customer> GetByIdAsync(int customerId);

    }
}
