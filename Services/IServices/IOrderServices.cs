using Models.Orders;
using Sufra_MVC.Models.CustomerModels;
using Sufra_MVC.Models.Orders;

namespace Sufra_MVC.Services.IServices
{
    public interface IOrderServices
    {
        Task CreateOrderByCustomeridAsync();
    }
}
