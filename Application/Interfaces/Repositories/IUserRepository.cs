using EyeOfHorusP.Application.DTOs.Auth;
using EyeOfHorusP.Application.DTOs.User;
using EyeOfHorusP.Domain.Entities;
using System.Threading.Tasks;

namespace EyeOfHorusP.Application.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task<bool> IsUniqueUserName(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterRequestDTO registerRequestDTO);
        Task<ApplicationUser> GetUserById(string userId); // تعديل الاسم ليتماشى مع UserService
        Task<bool> UpdateAsync(ApplicationUser user);
    }
}
