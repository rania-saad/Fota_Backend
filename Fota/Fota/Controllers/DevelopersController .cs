using AutoMapper;
using Fota.BusinessLayer.Interfaces;
using Fota.BusinessLayer.Repositories;
using Fota.BusinessLayer.Repositories;
using Fota.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedProjectDTOs.Admin;
using SharedProjectDTOs.Developers;
using System.Security.Claims;
namespace StaffAffairs.AWebAPI.Controllers
{
    //[Authorize(Roles = "ADMIN,DEVELOPER")]

    [ApiController]
    [Route("api/[controller]")]
    public class DevelopersController : ControllerBase
    {
        private readonly IDeveloperRepository _developerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DevelopersController> _logger;



        public DevelopersController(IDeveloperRepository developerRepository, IMapper mapper, ILogger<DevelopersController> logger)
        {
            _developerRepository = developerRepository;
            _mapper = mapper;
            _logger = logger;
        }


        private async Task<int> GetCurrentDeveloperIdAsync()
        {
            // ✅ جرب أكثر من Claim Type
            var identityUserId = User.FindFirst("uid")?.Value
                              ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(identityUserId))
            {
                _logger.LogWarning("⚠️ User is not authenticated - no uid or NameIdentifier claim found");
                throw new UnauthorizedAccessException("User is not authenticated");
            }

            _logger.LogInformation("🔍 Looking for developer with IdentityUserId: {UserId}", identityUserId);

            var developerId = await _developerRepository
                .GetDeveloperIdByIdentityUserIdAsync(identityUserId);

            if (!developerId.HasValue)
            {
                _logger.LogWarning("⚠️ Developer not found for IdentityUserId: {UserId}", identityUserId);
                throw new UnauthorizedAccessException("Developer not found for this user");
            }

            _logger.LogInformation("✅ Found DeveloperId: {DeveloperId}", developerId.Value);
            return developerId.Value;
        }

        /// <summary>
        /// ✅ Helper: استخراج User Identity ID فقط
        /// </summary>
        private string GetCurrentUserIdentityId()
        {
            var identityUserId = User.FindFirst("uid")?.Value
                              ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(identityUserId))
            {
                _logger.LogWarning("⚠️ User Identity ID not found in claims");
                throw new UnauthorizedAccessException("User is not authenticated");
            }

            return identityUserId;
        }

        /// <summary>
        /// ✅ Helper: التحقق من الـ Roles
        /// </summary>
        private bool IsAdmin()
        {
            var isAdmin = User.IsInRole("ADMIN");
            _logger.LogInformation("👤 User is admin: {IsAdmin}", isAdmin);
            return isAdmin;
        }

        private bool IsDeveloper()
        {
            var isDeveloper = User.IsInRole("DEVELOPER");
            _logger.LogInformation("👤 User is developer: {IsDeveloper}", isDeveloper);
            return isDeveloper;
        }

        // GET: api/Developers


        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeveloperGetDTO>>> GetAllDevelopers()
        {

            try
            {
                var Developers = await _developerRepository.GetAllAsync();

                if (Developers == null || !Developers.Any())
                {
                    return NoContent(); // Not found
                }

                var DeveloperDTOs = _mapper.Map<List<DeveloperGetDTO>>(Developers);
                return Ok(DeveloperDTOs);
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

        // GET: api/Developers/5

        [HttpGet("{id}")]
        public async Task<ActionResult<DeveloperGetDTO>> GetDeveloperById(int id)
        {


            try
            {
                var Developer = await _developerRepository.GetByIdAsync(id);

                if (Developer == null)
                {
                    return NotFound(new { message = $"Developer with ID {id} not found" });
                }

                var DeveloperDTO = _mapper.Map<DeveloperGetDTO>(Developer);
                return Ok(DeveloperDTO);
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



        // GET: api/Developers/search?name=mohamed
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Developer>>> SearchDevelopers([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Search name cannot be empty" });

            var developers = await _developerRepository.SearchByNameAsync(name);
            return Ok(developers);
        }


        // GET: api/Developers/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Developer>>> GetActiveDevelopers()
        {
            var developers = await _developerRepository.GetActiveDevelopersAsync();
            return Ok(developers);
        }



        // GET: api/Developers/team/1
        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<IEnumerable<DeveloperTeamDto>>> GetDevelopersByTeam(int teamId)
        {
            var developers = await _developerRepository.GetByTeamAsync(teamId);
            var DeveloperDTOs = _mapper.Map<List<DeveloperTeamDto>>(developers);
            return Ok(DeveloperDTOs);
        }


        [HttpPost]
        public async Task<ActionResult<Developer>> CreateDeveloper([FromBody] DeveloperCreateDto dto)
        {
            var existing = await _developerRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                return Conflict(new { message = "Email already exists" });

            var developer = new Developer
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _developerRepository.AddAsync(developer);
            return CreatedAtAction(nameof(GetDeveloperById), new { id = created.Id }, created);
        }



        // PUT: api/Developers/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Developer>> UpdateDeveloper(int id, [FromBody] DeveloperUpdateDto dto)
        {
            var developer = await _developerRepository.GetByIdAsync(id);
            if (developer == null)
                return NotFound(new { message = $"Developer with ID {id} not found" });

            developer.Name = dto.Name;
            developer.PhoneNumber = dto.PhoneNumber;
            developer.IsActive = dto.IsActive;
            developer.UpdatedAt = DateTime.UtcNow;

            var updated = await _developerRepository.UpdateAsync(developer);
            return Ok(updated);
        }


        // DELETE: api/Developers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDeveloper(int id)
        {
            var deleted = await _developerRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Developer with ID {id} not found" });

            return NoContent();
        }


        //[HttpGet("{id}/teams")]
        //public async Task<ActionResult<IEnumerable<object>>> GetDeveloperTeams(int id)
        //{
        //    // First check if developer exists
        //    var developer = await _developerRepository.GetByIdAsync(id);
        //    if (developer == null)
        //    {
        //        return NotFound(new { message = $"Developer with ID {id} not found." });
        //    }

        //    // Get all teams the developer belongs to
        //    var teams = await _developerRepository.GetTeamsByDeveloperIdAsync(id);

        //    // Transform the data to return a clean response
        //    var teamDtos = teams.Select(team => new
        //    {
        //        id = team.Id,
        //        name = team.Name,
        //        description = team.Description,
        //        isActive = team.IsActive,
        //        memberCount = team.TeamDevelopers?.Count(td => td.IsActive) ?? 0,
        //        topicCount = team.TeamTopics?.Count ?? 0,
        //        teamLead = team.Lead != null ? new
        //        {
        //            id = team.Lead.Id,
        //            name = team.Lead.Name,
        //            email = team.Lead.Email
        //        } : null,
        //        assignedTopics = team.TeamTopics?.Select(tt => tt.Topic?.Name).ToList() ?? new List<string>(),
        //        createdAt = team.CreatedAt,
        //        updatedAt = team.UpdatedAt
        //    }).ToList();

        //    return Ok(new
        //    {
        //        developerId = id,
        //        developerName = developer.Name,
        //        totalTeams = teamDtos.Count,
        //        teams = teamDtos
        //    });
        //}

        // GET: api/Developers/my/teams
        [HttpGet("my/teams")]
        public async Task<ActionResult<IEnumerable<object>>> GetMyTeams()
        {
            try
            {
                // Get developerId from token
                int developerId = await GetCurrentDeveloperIdAsync();

                // First check if developer exists
                var developer = await _developerRepository.GetByIdAsync(developerId);
                if (developer == null)
                {
                    return NotFound(new { message = $"Developer with ID {developerId} not found." });
                }

                // Get all teams the developer belongs to
                var teams = await _developerRepository.GetTeamsByDeveloperIdAsync(developerId);

                // Transform the data to return a clean response
                var teamDtos = teams.Select(team => new
                {
                    id = team.Id,
                    name = team.Name,
                    description = team.Description,
                    isActive = team.IsActive,
                    memberCount = team.TeamDevelopers?.Count(td => td.IsActive) ?? 0,
                    topicCount = team.TeamTopics?.Count ?? 0,
                    teamLead = team.Lead != null ? new
                    {
                        id = team.Lead.Id,
                        name = team.Lead.Name,
                        email = team.Lead.Email
                    } : null,
                    assignedTopics = team.TeamTopics?.Select(tt => tt.Topic?.Name).ToList() ?? new List<string>(),
                    createdAt = team.CreatedAt,
                    updatedAt = team.UpdatedAt
                }).ToList();

                return Ok(new
                {
                    developerId = developerId,
                    developerName = developer.Name,
                    totalTeams = teamDtos.Count,
                    teams = teamDtos
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching developer teams");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred." });
            }
        }

    }
}


//using AutoMapper;
//using Fota.BusinessLayer.Interfaces;
//using Fota.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using SharedProjectDTOs.Developers;
//using System.Security.Claims;

//namespace StaffAffairs.AWebAPI.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    [Authorize(Roles = "ADMIN,DEVELOPER")]
//    // ✅ Global authentication
//    public class DevelopersController : ControllerBase
//    {
//        private readonly IDeveloperRepository _developerRepository;
//        private readonly IMapper _mapper;
//        private readonly ILogger<DevelopersController> _logger;

//        public DevelopersController(
//            IDeveloperRepository developerRepository,
//            IMapper mapper,
//            ILogger<DevelopersController> logger)
//        {
//            _developerRepository = developerRepository;
//            _mapper = mapper;
//            _logger = logger;
//        }

//        // =========================
//        // Helpers (unchanged)
//        // =========================

//        private async Task<int> GetCurrentDeveloperIdAsync()
//        {
//            var identityUserId = User.FindFirst("uid")?.Value
//                              ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (string.IsNullOrEmpty(identityUserId))
//                throw new UnauthorizedAccessException("User is not authenticated");

//            var developerId =
//                await _developerRepository.GetDeveloperIdByIdentityUserIdAsync(identityUserId);

//            if (!developerId.HasValue)
//                throw new UnauthorizedAccessException("Developer not found for this user");

//            return developerId.Value;
//        }


//        private string GetCurrentUserIdentityId()
//        {
//            var identityUserId = User.FindFirst("uid")?.Value
//                              ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (string.IsNullOrEmpty(identityUserId))
//                throw new UnauthorizedAccessException("User is not authenticated");

//            return identityUserId;
//        }

//        private bool IsAdmin() => User.IsInRole("ADMIN");
//        private bool IsDeveloper() => User.IsInRole("DEVELOPER");

//        // =========================
//        // GET ALL
//        // =========================
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<DeveloperGetDTO>>> GetAllDevelopers()
//        {
//            try
//            {
//                var userId = GetCurrentUserIdentityId();
//                _logger.LogInformation("📋 User {UserId} requesting all developers", userId);

//                var developers = await _developerRepository.GetAllAsync();
//                if (developers == null || !developers.Any())
//                    return NoContent();

//                return Ok(_mapper.Map<List<DeveloperGetDTO>>(developers));
//            }
//            catch (UnauthorizedAccessException ex)
//            {
//                _logger.LogWarning(ex, "🔒 Unauthorized access");
//                return Unauthorized(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "❌ Error getting developers");
//                return StatusCode(500, new { message = "An unexpected error occurred" });
//            }
//        }

//        // =========================
//        // GET BY ID
//        // =========================
//        [HttpGet("{id}")]
//        public async Task<ActionResult<DeveloperGetDTO>> GetDeveloperById(int id)
//        {
//            try
//            {
//                var developer = await _developerRepository.GetByIdAsync(id);
//                if (developer == null)
//                    return NotFound(new { message = $"Developer with ID {id} not found" });

//                return Ok(_mapper.Map<DeveloperGetDTO>(developer));
//            }
//            catch (UnauthorizedAccessException ex)
//            {
//                return Unauthorized(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "❌ Error getting developer {Id}", id);
//                return StatusCode(500, new { message = "An unexpected error occurred" });
//            }
//        }

//        // =========================
//        // SEARCH
//        // =========================
//        [HttpGet("search")]
    
//        public async Task<ActionResult> SearchDevelopers([FromQuery] string name)
//        {
//            if (string.IsNullOrWhiteSpace(name))
//                return BadRequest(new { message = "Search name cannot be empty" });

//            try
//            {
//                var developers = await _developerRepository.SearchByNameAsync(name);
//                return Ok(developers);
//            }
//            catch (UnauthorizedAccessException ex)
//            {
//                return Unauthorized(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "❌ Error searching developers");
//                return StatusCode(500, new { message = "An unexpected error occurred" });
//            }
//        }

//        // =========================
//        // CREATE
//        // =========================
//        [HttpPost]
//        public async Task<ActionResult> CreateDeveloper([FromBody] DeveloperCreateDto dto)
//        {
//            try
//            {
//                var existing = await _developerRepository.GetByEmailAsync(dto.Email);
//                if (existing != null)
//                    return Conflict(new { message = "Email already exists" });

//                var developer = new Developer
//                {
//                    Name = dto.Name,
//                    Email = dto.Email,
//                    PhoneNumber = dto.PhoneNumber,
//                    IsActive = true,
//                    CreatedAt = DateTime.UtcNow
//                };

//                var created = await _developerRepository.AddAsync(developer);
//                return CreatedAtAction(nameof(GetDeveloperById),
//                    new { id = created.Id }, created);
//            }
//            catch (UnauthorizedAccessException ex)
//            {
//                return Unauthorized(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "❌ Error creating developer");
//                return StatusCode(500, new { message = "An unexpected error occurred" });
//            }
//        }

//        // =========================
//        // UPDATE
//        // =========================
//        [HttpPut("{id}")]
//        public async Task<ActionResult> UpdateDeveloper(int id, [FromBody] DeveloperUpdateDto dto)
//        {
//            try
//            {
//                var developer = await _developerRepository.GetByIdAsync(id);
//                if (developer == null)
//                    return NotFound(new { message = $"Developer with ID {id} not found" });

//                developer.Name = dto.Name;
//                developer.PhoneNumber = dto.PhoneNumber;
//                developer.IsActive = dto.IsActive;
//                developer.UpdatedAt = DateTime.UtcNow;

//                var updated = await _developerRepository.UpdateAsync(developer);
//                return Ok(updated);
//            }
//            catch (UnauthorizedAccessException ex)
//            {
//                return Unauthorized(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "❌ Error updating developer {Id}", id);
//                return StatusCode(500, new { message = "An unexpected error occurred" });
//            }
//        }

//        // =========================
//        // DELETE
//        // =========================
//        [HttpDelete("{id}")]
//        public async Task<ActionResult> DeleteDeveloper(int id)
//        {
//            try
//            {
//                var deleted = await _developerRepository.DeleteAsync(id);
//                if (!deleted)
//                    return NotFound(new { message = $"Developer with ID {id} not found" });

//                return NoContent();
//            }
//            catch (UnauthorizedAccessException ex)
//            {
//                return Unauthorized(new { message = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "❌ Error deleting developer {Id}", id);
//                return StatusCode(500, new { message = "An unexpected error occurred" });
//            }
//        }
//    }
//}
