//using Fota.BusinessLayer.Interfaces;
//using Fota.DataLayer.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using SharedProjectDTOs.Teams;

//namespace Fota.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class TeamsController : ControllerBase
//    {
//        private readonly ITeamRepository _teamRepository;

//        public TeamsController(ITeamRepository teamRepository)
//        {
//            _teamRepository = teamRepository;
//        }

//        // GET: api/Teams
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Team>>> GetAllTeams()
//        {
//            var teams = await _teamRepository.GetAllAsync();
//            return Ok(teams);
//        }

//        // GET: api/Teams/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Team>> GetTeamById(int id)
//        {
//            var team = await _teamRepository.GetByIdAsync(id);
//            if (team == null)
//                return NotFound(new { message = $"Team with ID {id} not found" });

//            return Ok(team);
//        }

//        // GET: api/Teams/search?name=ECU
//        [HttpGet("search")]
//        public async Task<ActionResult<IEnumerable<Team>>> SearchTeams([FromQuery] string name)
//        {
//            if (string.IsNullOrWhiteSpace(name))
//                return BadRequest(new { message = "Search name cannot be empty" });

//            var teams = await _teamRepository.SearchByNameAsync(name);
//            return Ok(teams);
//        }

//        // GET: api/Teams/active
//        [HttpGet("active")]
//        public async Task<ActionResult<IEnumerable<Team>>> GetActiveTeams()
//        {
//            var teams = await _teamRepository.GetActiveTeamsAsync();
//            return Ok(teams);
//        }

//        // GET: api/Teams/lead/1
//        [HttpGet("lead/{leadId}")]
//        public async Task<ActionResult<IEnumerable<Team>>> GetTeamsByLead(int leadId)
//        {
//            var teams = await _teamRepository.GetByLeadAsync(leadId);
//            return Ok(teams);
//        }

//        // POST: api/Teams
//        [HttpPost]
//        public async Task<ActionResult<Team>> CreateTeam([FromBody] TeamCreateDto dto)
//        {
//            var team = new Team
//            {
//                Name = dto.Name,
//                Description = dto.Description,
//                LeadId = dto.LeadId,
//                CreatedByAdminId = dto.CreatedByAdminId,
//                IsActive = true,
//                CreatedAt = DateTime.UtcNow
//            };

//            var created = await _teamRepository.AddAsync(team);
//            return CreatedAtAction(nameof(GetTeamById), new { id = created.Id }, created);
//        }

//        // PUT: api/Teams/5
//        [HttpPut("{id}")]
//        public async Task<ActionResult<Team>> UpdateTeam(int id, [FromBody] TeamUpdateDto dto)
//        {
//            var team = await _teamRepository.GetByIdAsync(id);
//            if (team == null)
//                return NotFound(new { message = $"Team with ID {id} not found" });

//            team.Name = dto.Name;
//            team.Description = dto.Description;
//            team.LeadId = dto.LeadId;
//            team.IsActive = dto.IsActive;
//            team.UpdatedAt = DateTime.UtcNow;

//            var updated = await _teamRepository.UpdateAsync(team);
//            return Ok(updated);
//        }

//        // DELETE: api/Teams/5
//        [HttpDelete("{id}")]
//        public async Task<ActionResult> DeleteTeam(int id)
//        {
//            var deleted = await _teamRepository.DeleteAsync(id);
//            if (!deleted)
//                return NotFound(new { message = $"Team with ID {id} not found" });

//            return NoContent();
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using Fota.BusinessLayer.Interfaces;
using SharedProjectDTOs.Teams;
using AutoMapper;
using Fota.DataLayer.Models;
using Fota.BusinessLayer.Repositories;

namespace Fota.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase

    {
        private readonly IAdminRepository  _adminRepository;
        private readonly IDeveloperRepository _developerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;

        public TeamsController(ITeamRepository teamRepository, IMapper mapper, IAdminRepository adminRepository, IDeveloperRepository developerRepository)
        {
            _teamRepository = teamRepository;
            _mapper = mapper;
            _adminRepository = adminRepository;
            _developerRepository = developerRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamGetDTO>>> GetAllTeams()
        {
            var teams = await _teamRepository.GetAllAsync();
            return Ok(_mapper.Map<List<TeamGetDTO>>(teams));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeamGetDTO>> GetTeamById(int id)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null)
                return NotFound();

            return Ok(_mapper.Map<TeamGetDTO>(team));
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TeamGetDTO>>> SearchTeams(string name)
        {
            var teams = await _teamRepository.SearchByNameAsync(name);
            return Ok(_mapper.Map<List<TeamGetDTO>>(teams));
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<TeamGetDTO>>> GetActiveTeams()
        {
            var teams = await _teamRepository.GetActiveTeamsAsync();
            return Ok(_mapper.Map<List<TeamGetDTO>>(teams));
        }

        [HttpGet("lead/{leadId}")]
        public async Task<ActionResult<IEnumerable<TeamGetDTO>>> GetTeamsByLead(int leadId)
        {
            var teams = await _teamRepository.GetByLeadAsync(leadId);
            return Ok(_mapper.Map<List<TeamGetDTO>>(teams));
        }
        [HttpPost]
        public async Task<ActionResult<TeamGetDTO>> CreateTeam([FromBody] TeamCreateDto dto)
        {
            // Validate Lead exists
            if (dto.LeadId == null)
                return BadRequest(new { message = "LeadId is required" });

            if (!await _developerRepository.ExistsAsync(dto.LeadId.Value))
                return BadRequest(new { message = $"Developer with ID {dto.LeadId} not found" });

            if (dto.CreatedByAdminId == null)
                return BadRequest(new { message = "CreatedByAdminId is required" });

            if (!await _adminRepository.ExistsAsync(dto.CreatedByAdminId.Value))
                return BadRequest(new { message = $"Admin with ID {dto.CreatedByAdminId} not found" });

            // Validate Admin exists


            // Use AutoMapper AFTER validation
            var team = _mapper.Map<Team>(dto);
            team.IsActive = true;
            team.CreatedAt = DateTime.UtcNow;

            var created = await _teamRepository.AddAsync(team);
            return Ok(_mapper.Map<TeamGetDTO>(created));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TeamGetDTO>> UpdateTeam(int id, [FromBody] TeamUpdateDto dto)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team == null)
                return NotFound();

            team.Name = dto.Name;
            team.Description = dto.Description;
            team.LeadId = dto.LeadId;
            team.IsActive = dto.IsActive;
            team.UpdatedAt = DateTime.UtcNow;

            var updated = await _teamRepository.UpdateAsync(team);
            return Ok(_mapper.Map<TeamGetDTO>(updated));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeam(int id)
        {
            var deleted = await _teamRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
