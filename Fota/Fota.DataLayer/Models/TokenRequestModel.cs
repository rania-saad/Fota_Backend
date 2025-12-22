using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fota.DataLayer.Models
{
    public class TokenRequestModel
    {
        [Required, StringLength(150)]

        // Email must be unique
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(256)]

        // Password for login
        public string Password { get; set; } = string.Empty;

    }
}
