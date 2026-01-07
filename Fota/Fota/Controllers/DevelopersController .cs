using Fota.BusinessLayer.Repositories;
using Fota.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fota.BusinessLayer.Interfaces;
using Fota.BusinessLayer.Repositories;
using SharedProjectDTOs.Developers;
using AutoMapper;
using SharedProjectDTOs.Admin;
namespace StaffAffairs.AWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevelopersController : ControllerBase
    {
        private readonly IDeveloperRepository _developerRepository;
        private readonly IMapper _mapper;


        public DevelopersController(IDeveloperRepository developerRepository, IMapper mapper)
        {
            _developerRepository = developerRepository;
            _mapper = mapper;
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

            // POST: api/Developers
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


        [HttpGet("{id}/teams")]
        public async Task<ActionResult<IEnumerable<object>>> GetDeveloperTeams(int id)
        {
            // First check if developer exists
            var developer = await _developerRepository.GetByIdAsync(id);
            if (developer == null)
            {
                return NotFound(new { message = $"Developer with ID {id} not found." });
            }

            // Get all teams the developer belongs to
            var teams = await _developerRepository.GetTeamsByDeveloperIdAsync(id);

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
                developerId = id,
                developerName = developer.Name,
                totalTeams = teamDtos.Count,
                teams = teamDtos
            });
        }
    }
    }