using Microsoft.AspNetCore.Mvc;
using Fota.BusinessLayer.Interfaces;
using Fota.DataLayer.Models;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SharedProjectDTOs.Admin;


namespace Fota.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;

        public AdminsController(IAdminRepository adminRepository, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
        }

        // GET: api/Admins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminGetDTO>>> GetAllAdmins()
        {

            try
            {
                var admins = await _adminRepository.GetAllAsync();

                if (admins == null || !admins.Any())
                {
                    return NoContent(); // Not found
                }

                var adminDTOs = _mapper.Map<List<AdminGetDTO>>(admins);
                return Ok(adminDTOs);
            }
            
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request",
                    }
                );
            }
           
        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminGetDTO>> GetAdminById(int id)
        {


            try
            {
                var admin = await _adminRepository.GetByIdAsync(id);

                if (admin == null )
                {
                    return NotFound(new { message = $"Admin with ID {id} not found" });
                }

                var adminDTO = _mapper.Map<AdminGetDTO>(admin);
                return Ok(adminDTO);
            }

            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request",
                    }
                );
            }

          
              

        }

        // GET: api/Admins/search?name=sarah
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Admin>>> SearchAdmins([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Search name cannot be empty" });

            var admins = await _adminRepository.SearchByNameAsync(name);
            return Ok(admins);
        }

        // GET: api/Admins/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Admin>>> GetActiveAdmins()
        {
            var admins = await _adminRepository.GetActiveAdminsAsync();
            return Ok(admins);
        }

        // POST: api/Admins
        [HttpPost]
        public async Task<ActionResult<Admin>> CreateAdmin([FromBody] AdminCreateDto dto)
        {
            // Check if email already exists
            var existing = await _adminRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                return Conflict(new { message = "Email already exists" });

            var admin = new Admin
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _adminRepository.AddAsync(admin);
            return CreatedAtAction(nameof(GetAdminById), new { id = created.Id }, created);
        }

        // PUT: api/Admins/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Admin>> UpdateAdmin(int id, [FromBody] AdminUpdateDto dto)
        {
            var admin = await _adminRepository.GetByIdAsync(id);
            if (admin == null)
                return NotFound(new { message = $"Admin with ID {id} not found" });

            admin.Name = dto.Name;
            admin.PhoneNumber = dto.PhoneNumber;
            admin.IsActive = dto.IsActive;
            admin.UpdatedAt = DateTime.UtcNow;

            var updated = await _adminRepository.UpdateAsync(admin);
            return Ok(updated);
        }

        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAdmin(int id)
        {
            var deleted = await _adminRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Admin with ID {id} not found" });

            return NoContent();
        }
    }
}
