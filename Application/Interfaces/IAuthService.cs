using EyeOfHorusP.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeOfHorusP.Application.Interfaces
{
    public interface IAuthService
    {
        Task<object> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<object> RegisterAsync(RegisterRequestDTO registerRequestDTO);
    }
}
