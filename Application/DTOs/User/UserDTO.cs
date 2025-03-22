﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeOfHorusP.Application.DTOs.User
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
