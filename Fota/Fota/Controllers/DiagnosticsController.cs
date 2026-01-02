using Fota.BusinessLayer.Interfaces;
using Fota.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using SharedProjectDTOs.DiagnosticDTOs;
using SharedProjectDTOs.DiagnosticDTOs.SharedProjectDTOs.Diagnostics;
using Fota.DataLayer.Enum;
namespace StaffAffairs.AWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiagnosticsController : ControllerBase
    {
        private readonly IDiagnosticRepository _diagnosticRepository;
        private readonly IMapper _mapper;

        public DiagnosticsController(IDiagnosticRepository diagnosticRepository, IMapper mapper)
        {
            _diagnosticRepository = diagnosticRepository;
            _mapper = mapper;
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

        // GET: api/Diagnostics/developer/5
        [HttpGet("developer/{developerId}")]
        public async Task<ActionResult<IEnumerable<DiagnosticListDto>>> GetDiagnosticsByDeveloper(int developerId)
        {
            try
            {
                var diagnostics = await _diagnosticRepository.GetByAssignedDeveloperAsync(developerId);
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

                if (diagnostic.Status == DiagnosticStatus.Closed)
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
    }
}