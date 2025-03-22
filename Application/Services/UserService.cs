using EyeOfHorusP.Application.Interfaces;
using EyeOfHorusP.Application.Interfaces.Repositories;
using EyeOfHorusP.Domain.Entities;

namespace EyeOfHorusP.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _userRepo.GetUserById(userId);
        }
    }
}

