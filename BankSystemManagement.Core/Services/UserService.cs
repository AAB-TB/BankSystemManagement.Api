using AutoMapper;
using BankSystemManagement.Core.DTOs;
using BankSystemManagement.Core.Interfaces;
using BankSystemManagement.Data.Interfaces;
using Microsoft.Extensions.Logging;



namespace BankSystemManagement.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepo userRepo, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int?> CustomerRegistrationAsync(CustomerRegistrationDto registrationDto)
        {
            return await _userRepo.CustomerRegistrationAsync(registrationDto);
        }

        public async Task<bool> AdminRegistrationAsync(AdminRegistrationDto registrationDTO)
        {
            return await _userRepo.AdminRegistrationAsync(registrationDTO);
        }

        public async Task<CustomerDetailsDto> GetCustomerDetailsAsync(int customerId)
        {
            return await _userRepo.GetCustomerDetailsAsync(customerId);
        }

        public async Task<string> LoginUserAsync(string username, string password)
        {
            return await _userRepo.LoginUserAsync(username, password);
        }
    }
}
