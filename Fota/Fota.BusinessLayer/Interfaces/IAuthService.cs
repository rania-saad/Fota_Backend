using Fota.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fota.BusinessLayer.Interfaces
{
    public interface IAuthService
    {
       // Task<string> LoginAsync(string email, string password);
        Task<AuthModel> RegisterAsync(RegisterModel Model);
        // Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user);
        Task<AuthModel> GetTokenAsync(TokenRequestModel Model);
        Task<string> AddRoleAsync(AddRole Model);
    }
}
