using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fota.BusinessLayer.Interfaces;
using Fota.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Fota.BusinessLayer.Interfaces;
using Fota.DataLayer.Models;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Fota.BusinessLayer.Services;
using Microsoft.AspNetCore.Http;

namespace Fota.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;


        public AuthController(IAuthService authService , ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

          //  return Ok(result);
          return Ok(new { token = result.Token ,Erpires =result.Expiration });

        }

        [HttpPost("Login")]

        public async Task<IActionResult> LoginAsync([FromBody] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            //  return Ok(result);
            return Ok(new { token = result.Token, Erpires = result.Expiration });

        }

        


       [HttpPost("AssignRole")]

        public async Task<IActionResult> AddRoleAsync([FromBody] AddRole model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            //  return Ok(result);
            return Ok(model);

        }





    }
}
