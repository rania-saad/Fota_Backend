using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fota.DataLayer.Models
{
    public class RegisterModel
    {
        [Required, StringLength(100)]
        // Full name of the user
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required, StringLength(150)]

        // Email must be unique
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(256)]

        // Password for login
        public string Password { get; set; } = string.Empty;

        // Confirm password must match password
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required, StringLength(100)]

        // Optional phone number
        public string? PhoneNumber { get; set; }

        // Optional address
        public string? Address { get; set; }
    }
}
