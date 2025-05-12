using Sufra_MVC.Data;
using Sufra_MVC.Repositories;
using Sufra_MVC.Repositories.IRepositories;
using Sufra_MVC.Services.IServices;

namespace Sufra_MVC.Services.Services
{
    public class OrderServices:IOrderServices
    {
        private readonly IOrderRepository _orderRepository;
        public OrderServices(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        //-----------------------------------------

        public async Task CreateOrderByCustomeridAsync()
        {

        }

    }
}

