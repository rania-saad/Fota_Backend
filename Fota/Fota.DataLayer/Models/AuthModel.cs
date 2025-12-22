using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fota.DataLayer.Models
{
    public class AuthModel
    {
        public string Message { get; set; }

        // false by default 
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }
        public IList<string> Roles { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }


    }

}
