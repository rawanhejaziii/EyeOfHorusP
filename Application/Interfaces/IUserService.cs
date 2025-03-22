
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EyeOfHorusP.Domain.Entities;

namespace EyeOfHorusP.Application.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserById(string userId);

    }
}
