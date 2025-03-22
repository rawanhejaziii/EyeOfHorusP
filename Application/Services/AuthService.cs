using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EyeOfHorusP.Application.DTOs.Auth;
using EyeOfHorusP.Application.Interfaces.Repositories;
using EyeOfHorusP.Application.Interfaces;

namespace EyeOfHorusP.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        private void TestSecretKey()
        {
            string secretKey = _configuration["ApiSettings:Secret"];
            Console.WriteLine($"✅ Secret Key: {secretKey}");
        }

        public async Task<object> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            TestSecretKey(); // 🔥 تأكد أن الـ Secret Key يتم تحميله
            return await _userRepository.Login(loginRequestDTO);
        }

        public async Task<object> RegisterAsync(RegisterRequestDTO registerRequestDTO)
        {
            TestSecretKey(); // 🔥 تأكد أن الـ Secret Key يتم تحميله

            var emailExist = await _userRepository.GetAsync(user => user.Email == registerRequestDTO.Email);
            if (emailExist != null)
            {
                throw new ValidationException("Email Already exists");
            }

            return await _userRepository.Register(registerRequestDTO);
        }
    }
}
