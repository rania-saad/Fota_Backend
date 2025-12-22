using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fota.DataLayer.Models
{
    public class JwtSettings
    {
        // Secret Key used to sign the token
        public string Key { get; set; }

        // Who issues the token
        public string Issuer { get; set; }

        // Who can accept the token
        public string Audience { get; set; }

        // Token validity duration in minutes
        public Double DurationInMinutes { get; set; }
    }


}
