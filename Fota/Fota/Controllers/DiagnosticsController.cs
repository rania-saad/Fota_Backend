using AutoMapper;
using Fota.BusinessLayer.Interfaces;
using Fota.BusinessLayer.Repositories;
using Fota.DataLayer.Enum;
using Fota.DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedProjectDTOs.DiagnosticDTOs;
using SharedProjectDTOs.DiagnosticDTOs.SharedProjectDTOs.Diagnostics;
using System.Security.Claims;
namespace StaffAffairs.AWebAPI.Controllers
{
    //[Authorize(Roles = "ADMIN,DEVELOPER")]

    [ApiController]
    [Route("api/[controller]")]
    public class DiagnosticsController : ControllerBase
    {
        private readonly IDiagnosticRepository _diagnosticRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BaseMessagesController> _logger;
        private readonly IDeveloperRepository _developerRepository;


        public DiagnosticsController(IDiagnosticRepository diagnosticRepository, IMapper mapper, ILogger<BaseMessagesController> logger, IDeveloperRepository developerRepository)
        {
            _diagnosticRepository = diagnosticRepository;
            _mapper = mapper;
            _logger = logger;
            _developerRepository = developerRepository;
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





        // GET: api/Diagnostics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiagnosticListDto>>> GetAllDiagnostics()
        {
            try
            {
                var diagnostics = await _diagnosticRepository.GetAllAsync();

                if (diagnostics == null || !diagnostics.Any())
                {
                    return NoContent();
                }

                var diagnosticDtos = _mapper.Map<List<DiagnosticListDto>>(diagnostics);
                return Ok(diagnosticDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // GET: api/Diagnostics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DiagnosticGetDto>> GetDiagnosticById(int id)
        {
            try
            {
                var diagnostic = await _diagnosticRepository.GetByIdAsync(id);

                if (diagnostic == null)
                {
                    return NotFound(new { message = $"Diagnostic with ID {id} not found" });
                }

                var diagnosticDto = _mapper.Map<DiagnosticGetDto>(diagnostic);
                return Ok(diagnosticDto);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // GET: api/Diagnostics/search?name=connection
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<DiagnosticListDto>>> SearchDiagnostics([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Search name cannot be empty" });

            try
            {
                var diagnostics = await _diagnosticRepository.SearchByNameAsync(name);
                var diagnosticDtos = _mapper.Map<List<DiagnosticListDto>>(diagnostics);
                return Ok(diagnosticDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // GET: api/Diagnostics/status/Open
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<DiagnosticListDto>>> GetDiagnosticsByStatus(DiagnosticStatus status)
        {
            try
            {
                var diagnostics = await _diagnosticRepository.GetByStatusAsync(status);
                var diagnosticDtos = _mapper.Map<List<DiagnosticListDto>>(diagnostics);
                return Ok(diagnosticDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // GET: api/Diagnostics/priority/Critical
        [HttpGet("priority/{priority}")]
        public async Task<ActionResult<IEnumerable<DiagnosticListDto>>> GetDiagnosticsByPriority(DiagnosticPriority priority)
        {
            try
            {
                var diagnostics = await _diagnosticRepository.GetByPriorityAsync(priority);
                var diagnosticDtos = _mapper.Map<List<DiagnosticListDto>>(diagnostics);
                return Ok(diagnosticDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // GET: api/Diagnostics/subscriber/5
        [HttpGet("subscriber/{subscriberId}")]
        public async Task<ActionResult<IEnumerable<DiagnosticListDto>>> GetDiagnosticsBySubscriber(int subscriberId)
        {
            try
            {
                var diagnostics = await _diagnosticRepository.GetBySubscriberAsync(subscriberId);
                var diagnosticDtos = _mapper.Map<List<DiagnosticListDto>>(diagnostics);
                return Ok(diagnosticDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // GET: api/Diagnostics/topic/5
        [HttpGet("topic/{topicId}")]
        public async Task<ActionResult<IEnumerable<DiagnosticListDto>>> GetDiagnosticsByTopic(int topicId)
        {
            try
            {
                var diagnostics = await _diagnosticRepository.GetByTopicAsync(topicId);
                var diagnosticDtos = _mapper.Map<List<DiagnosticListDto>>(diagnostics);
                return Ok(diagnosticDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // GET: api/Diagnostics/my
        [HttpGet("my")]
        [Authorize(Roles = "ADMIN,DEVELOPER")]
        public async Task<ActionResult<IEnumerable<DiagnosticListDto>>> GetMyDiagnostics()
        {
            try
            {
                // ✅ DeveloperId جاي من الـ Token
                var developerId = await GetCurrentDeveloperIdAsync();

                _logger.LogInformation("👤 Developer {DeveloperId} requesting assigned diagnostics", developerId);

                var diagnostics =
                    await _diagnosticRepository.GetByAssignedDeveloperAsync(developerId);

                var diagnosticDtos = _mapper.Map<List<DiagnosticListDto>>(diagnostics);

                return Ok(diagnosticDtos);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "🔒 Unauthorized access to developer diagnostics");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error getting diagnostics for current developer");
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" }
                );
            }
        }

        

        // GET: api/Diagnostics/open
        [HttpGet("open")]
        public async Task<ActionResult<IEnumerable<DiagnosticListDto>>> GetOpenDiagnostics()
        {
            try
            {
                var diagnostics = await _diagnosticRepository.GetOpenDiagnosticsAsync();
                var diagnosticDtos = _mapper.Map<List<DiagnosticListDto>>(diagnostics);
                return Ok(diagnosticDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // GET: api/Diagnostics/unassigned
        [HttpGet("unassigned")]
        public async Task<ActionResult<IEnumerable<DiagnosticListDto>>> GetUnassignedDiagnostics()
        {
            try
            {
                var diagnostics = await _diagnosticRepository.GetUnassignedDiagnosticsAsync();
                var diagnosticDtos = _mapper.Map<List<DiagnosticListDto>>(diagnostics);
                return Ok(diagnosticDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // GET: api/Diagnostics/daterange?startDate=2024-01-01&endDate=2024-12-31
        [HttpGet("daterange")]
        public async Task<ActionResult<IEnumerable<DiagnosticListDto>>> GetDiagnosticsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var diagnostics = await _diagnosticRepository.GetByDateRangeAsync(startDate, endDate);
                var diagnosticDtos = _mapper.Map<List<DiagnosticListDto>>(diagnostics);
                return Ok(diagnosticDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // POST: api/Diagnostics
        [HttpPost]
        public async Task<ActionResult<DiagnosticGetDto>> CreateDiagnostic([FromBody] DiagnosticCreateDto dto)
        {
            try
            {
                var diagnostic = new Diagnostic
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Priority = dto.Priority,
                    SubscriberId = dto.SubscriberId,
                    TopicId = dto.TopicId,
                    Status = DiagnosticStatus.Open,
                    CreatedAt = DateTime.UtcNow
                };

                var created = await _diagnosticRepository.AddAsync(diagnostic);
                var diagnosticDto = _mapper.Map<DiagnosticGetDto>(created);
                return CreatedAtAction(nameof(GetDiagnosticById), new { id = created.Id }, diagnosticDto);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // PUT: api/Diagnostics/5
        [HttpPut("{id}")]
        public async Task<ActionResult<DiagnosticGetDto>> UpdateDiagnostic(int id, [FromBody] DiagnosticUpdateDto dto)
        {
            try
            {
                var diagnostic = await _diagnosticRepository.GetByIdAsync(id);
                if (diagnostic == null)
                    return NotFound(new { message = $"Diagnostic with ID {id} not found" });

                if (diagnostic.Status == DiagnosticStatus.Resolved)
                    return BadRequest(new { message = "Cannot update a closed diagnostic" });

                if (!string.IsNullOrEmpty(dto.Title))
                    diagnostic.Title = dto.Title;
                if (dto.Description != null)
                    diagnostic.Description = dto.Description;
                if (dto.Priority==null)
                    diagnostic.Priority = dto.Priority;
                if (dto.TopicId.HasValue)
                    diagnostic.TopicId = dto.TopicId;

                diagnostic.UpdatedAt = DateTime.UtcNow;

                var updated = await _diagnosticRepository.UpdateAsync(diagnostic);
                var diagnosticDto = _mapper.Map<DiagnosticGetDto>(updated);
                return Ok(diagnosticDto);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // POST: api/Diagnostics/5/assign
        [HttpPost("{id}/assign")]
        public async Task<ActionResult<DiagnosticGetDto>> AssignDiagnostic(int id, [FromBody] DiagnosticAssignDto dto)
        {
            try
            {
                var diagnostic = await _diagnosticRepository.AssignToDeveloperAsync(id, dto.DeveloperId, dto.AdminId);
                if (diagnostic == null)
                    return NotFound(new { message = $"Diagnostic with ID {id} not found or is closed" });

                var diagnosticDto = _mapper.Map<DiagnosticGetDto>(diagnostic);
                return Ok(diagnosticDto);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // PUT: api/Diagnostics/5/status
        [HttpPut("{id}/status")]
        public async Task<ActionResult<DiagnosticGetDto>> UpdateDiagnosticStatus(int id, [FromBody] DiagnosticStatusUpdateDto dto)
        {
            try
            {
                var diagnostic = await _diagnosticRepository.UpdateStatusAsync(id, dto.Status);
                if (diagnostic == null)
                    return NotFound(new { message = $"Diagnostic with ID {id} not found" });

                var diagnosticDto = _mapper.Map<DiagnosticGetDto>(diagnostic);
                return Ok(diagnosticDto);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // POST: api/Diagnostics/5/resolve
        [HttpPost("{id}/resolve")]
        public async Task<ActionResult<DiagnosticGetDto>> ResolveDiagnostic(int id)
        {
            try
            {
                var diagnostic = await _diagnosticRepository.ResolveDiagnosticAsync(id);
                if (diagnostic == null)
                    return NotFound(new { message = $"Diagnostic with ID {id} not found or is closed" });

                var diagnosticDto = _mapper.Map<DiagnosticGetDto>(diagnostic);
                return Ok(diagnosticDto);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // POST: api/Diagnostics/5/close
        [HttpPost("{id}/close")]
        public async Task<ActionResult<DiagnosticGetDto>> CloseDiagnostic(int id)
        {
            try
            {
                var diagnostic = await _diagnosticRepository.CloseDiagnosticAsync(id);
                if (diagnostic == null)
                    return NotFound(new { message = $"Diagnostic with ID {id} not found" });

                var diagnosticDto = _mapper.Map<DiagnosticGetDto>(diagnostic);
                return Ok(diagnosticDto);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // DELETE: api/Diagnostics/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDiagnostic(int id)
        {
            try
            {
                var deleted = await _diagnosticRepository.DeleteAsync(id);
                if (!deleted)
                    return NotFound(new { message = $"Diagnostic with ID {id} not found" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "An unexpected error occurred while processing your request"
                    }
                );
            }
        }

        // GET: api/Diagnostics/TotalMessages
        [HttpGet("TotaDiagnostics")]
        public async Task<IActionResult> GetTotalDiagnostics()
        {
            var totalMessages = await _diagnosticRepository.GetTotalDiagnosticsCountAsync();
            return Ok(new { TotalDiagnostics = totalMessages });
        }
    }
}