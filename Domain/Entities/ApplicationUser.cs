using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace EyeOfHorusP.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

    }
}
