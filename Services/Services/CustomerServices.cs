using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;
using NuGet.Common;
using Sufra.Common.Enums;
using Sufra.Common.Types;
using Sufra.Data;
using Sufra.DTOs.CustomerDTOs;
using Sufra.Exceptions;
using Sufra.Infrastructure.Services;
using Sufra.Models;
using Sufra.Models.Customers;
using Sufra.Repositories.IRepositories;
using Sufra.Services.IServices;

namespace Sufra.Services.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly IRefreshTokenRepository _RefreshTokenRepository;
        private readonly ICustomerRepository _CustomerRepository;
        private readonly ITokenService _tokenService;
        


        public CustomerServices(ICustomerRepository customerRepository,IRefreshTokenRepository refreshTokenRepository,ITokenService tokenService)
        {
            _CustomerRepository = customerRepository;
            _RefreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
        }

        //------------------------------------------
        public async Task<RegisterResponseDTO> RegisterAsync(CustomerRegisterDTO registerDTO)
        {

            Customer customerWithSameEmail = await _CustomerRepository.GetCustomerByEmailAsync(registerDTO.Email);
            Customer customerWithSamePhone = await _CustomerRepository.GetCustomerByPhoneAsync(registerDTO.Phone);

            if (customerWithSameEmail != null)  throw new EmailAlreadyInUseException($"Email: {registerDTO.Email} is already in use.");

            if (customerWithSamePhone != null)  throw new PhoneAlreadyInUseException($"Number: {registerDTO.Phone} is already in use.");


            registerDTO.Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

            Customer customer = new Customer
            {
                Fname = registerDTO.Fname,
                Lname = registerDTO.Lname,
                Email = registerDTO.Email,
                Password = registerDTO.Password,
                Phone = registerDTO.Phone
            };

            await _CustomerRepository.AddCustomerAsync(customer);

            return new RegisterResponseDTO
            {
                Id = customer.Id,
                Email = customer.Email,
                Fname = customer.Fname,
                Lname = customer.Lname,
                Message = "Registration successful"
            };

        }


    }
}
