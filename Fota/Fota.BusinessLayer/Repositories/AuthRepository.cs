using Fota.BusinessLayer.Interfaces;
using Fota.BusinessLayer.Repositories;
using Fota.DataLayer.DBContext;
using Fota.DataLayer.Models;
using Fota.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fota.BusinessLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDeveloperRepository _developerRepository;
        private readonly JwtSettings _jwtSettings;

        private readonly FOTADbContext _context;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtOptions, RoleManager<IdentityRole> roleManager, IDeveloperRepository developerRepository, FOTADbContext context)
        {
            _userManager = userManager;
            _jwtSettings = jwtOptions.Value;
            _roleManager = roleManager;
            _developerRepository = developerRepository;
            _context = context;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            var emailExists = await _userManager.FindByEmailAsync(model.Email);
            if (emailExists != null)
                return new AuthModel { Message = "Email already registered", IsAuthenticated = false };

            var userNameExists = await _userManager.FindByNameAsync(model.UserName);
            if (userNameExists != null)
                return new AuthModel { Message = "Username already registered", IsAuthenticated = false };

            // mapping between ApplicationUser  and register model

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,

                PhoneNumber = model.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            var developer = new Developer
            {
                Name = model.UserName,
                Email = model.Email,  // 👈 ضروري
                PhoneNumber = model.PhoneNumber,
                IdentityUserId = user.Id
            };



            await _context.Developers.AddAsync(developer);
            await _context.SaveChangesAsync();


            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthModel { Message = errors, IsAuthenticated = false };
            }

            await _userManager.AddToRoleAsync(user, "DEVELOPER");

            var token = await GenerateJwtToken(user); // now returns string ✅

            return new AuthModel
            {
                Message = "Registration successful",
                IsAuthenticated = true,
                Email = user.Email,
                UserName = user.UserName,
                //Roles= new list <string> {"User}
                Roles = await _userManager.GetRolesAsync(user),
                // Expiration =jwtSecuritytoken.ValidTo;
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        public async Task<AuthModel> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                return new AuthModel
                {
                    Message = "Invalid email or password",
                    IsAuthenticated = false
                };

            var token = await GenerateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthModel
            {
                Message = "Login successful",
                IsAuthenticated = true,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes)
            };
        }

        // 🔐 Token generator returning string ✔
        //private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
        //{
        //    var roles = await _userManager.GetRolesAsync(user);
        //    var userClaims = await _userManager.GetClaimsAsync(user);
        //    var roleClaims = new List<Claim>();


        //    foreach (var role in roles)
        //    {
        //        roleClaims.Add(new Claim("roles", role));
        //    }
        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        //        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim("uid", user.Id)
        //    }
        //    .Union(userClaims)
        //    .Union(roleClaims)
        //    ;


        //    var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        //    var signingCredentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        //    var jwtSecuritytoken = new JwtSecurityToken(
        //        issuer: _jwtSettings.Issuer,
        //        audience: _jwtSettings.Audience,
        //        expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
        //        claims: claims,
        //        signingCredentials: signingCredentials
        //        );


        //    return jwtSecuritytoken;
        //    //return token ;
        //}

        private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roleClaims = new List<Claim>();

            // ✅ استخدم ClaimTypes.Role بدلاً من "roles"
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("uid", user.Id),
        new Claim(ClaimTypes.NameIdentifier, user.Id),  // ✅ إضافة NameIdentifier
        new Claim(ClaimTypes.Name, user.UserName)       // ✅ إضافة Name
    }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key)
            );
            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey,
                SecurityAlgorithms.HmacSha256
            );

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                claims: claims,
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }


        public async Task<AuthModel> GetTokenAsync(TokenRequestModel Model)
        {
            var AuthModel = new AuthModel();
            var user = await _userManager.FindByEmailAsync(Model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, Model.Password))
            {
                AuthModel.Message = "Email or Password is incorrect";
                return AuthModel;
            }

            var token = await GenerateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);


            AuthModel.IsAuthenticated = true;
            AuthModel.Email = user.Email;
            AuthModel.Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);
            AuthModel.UserName = user.UserName;
            AuthModel.Token = new JwtSecurityTokenHandler().WriteToken(token);
            AuthModel.Roles = roles.ToList();




            return AuthModel;
        }


         
        public async Task<string> AddRoleAsync(AddRole model)
        {
            // ❗هنا بدل ما ناخد UserId من TokenRequest، بناخد DeveloperId
            var developer = await _context.Developers.FindAsync(int.Parse(model.UserId)); // 👈 هو في الحقيقة DeveloperId
            if (developer == null || developer.IdentityUserId == null)
                return "Developer not found or not linked to Identity user";

            var user = await _userManager.FindByIdAsync(developer.IdentityUserId);
            if (user == null || !await _roleManager.RoleExistsAsync(model.RoleName.ToUpper()))
                return "Invalid Identity User or Role does not exist";

            if (await _userManager.IsInRoleAsync(user, model.RoleName))
                return "User already in this role";

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            return result.Succeeded ? string.Empty : "Something went wrong";
        }

    


    }
}

