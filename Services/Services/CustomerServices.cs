using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sufra.DTOs.CustomerDTOs;
using Sufra.Infrastructure.Services;
using Sufra.Models.Customers;
using Sufra.Repositories.IRepositories;
using Sufra.Services.IServices;

namespace Sufra.Services.Services
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ICustomerRepository _CustomerRepository;
        private readonly JwtServices _JwtService;


        public CustomerServices(ICustomerRepository Repo, JwtServices JwtService)
        {
            _CustomerRepository = Repo;
            _JwtService = JwtService;
        }

        //------------------------------------------
        public async Task<LoginResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            Customer customer = await _CustomerRepository.GetCustomerByEmailAsync(loginDTO.Email);

            if (customer == null)
                throw new InvalidOperationException("Invalid email or password.");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDTO.Password, customer.Password);

            if (!isPasswordValid)
                throw new InvalidOperationException("Invalid email or password.");



            var token = _JwtService.GenerateToken(
                new CustomerClaimsDTO
                {
                    userID = customer.Id,
                    Name = customer.Fname,
                    Email = customer.Email,
                    Role = "Customer"
                });

            return new LoginResponseDTO
            {
                Fname = customer.Fname,
                Lname = customer.Lname,
                Email = customer.Email,
                RoleforTesting = "Customer",
                Token = token
            };

        }

        public async Task<RegisterResponseDTO> RegisterAsync(RegisterDTO registerDTO)
        {

            Customer customerWithSameEmail = await _CustomerRepository.GetCustomerByEmailAsync(registerDTO.Email);
            Customer customerWithSamePhone = await _CustomerRepository.GetCustomerByPhoneAsync(registerDTO.phone);

            if (customerWithSameEmail != null)
            {
                throw new InvalidOperationException($"Email: {registerDTO.Email} is already in use.");
            }

            else if (customerWithSamePhone != null)
            {
                throw new InvalidOperationException($"Number: {registerDTO.phone} is already in use.");
            }

            registerDTO.Password = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

            Customer customer = new Customer
            {
                Fname = registerDTO.Fname,
                Lname = registerDTO.Lname,
                Email = registerDTO.Email,
                Password = registerDTO.Password,
                Phone = registerDTO.phone
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
