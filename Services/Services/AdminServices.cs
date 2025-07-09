using Sufra.Services.IServices;
using Sufra.Repositories.IRepositories;
using Sufra.Infrastructure.Services;
using Sufra.Models.Emps;
using Sufra.Exceptions;
using Sufra.DTOs.SufraEmpDTOs;

namespace Sufra.Services.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly ISufraEmpRepository _sufraEmpRepository;
        private readonly ITokenService _tokenService;


        public AdminServices(ISufraEmpRepository sufraEmpRepository,ITokenService tokenService)
        {  
            _sufraEmpRepository = sufraEmpRepository;
            _tokenService = tokenService;
        }

        //------------------------------------------

    }
}
