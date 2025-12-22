using AutoMapper;
using Fota.BusinessLayer.Interfaces;
using Fota.BusinessLayer.Repositories;
using Fota.DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedProjectDTOs.BaseMessages;
using SharedProjectDTOs.DiagnosticSolutionDTOs;

namespace StaffAffairs.AWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiagnosticSolutionsController : ControllerBase
    {
        private readonly IDiagnosticSolutionRepository _repo;
        private readonly IMapper _mapper;

        public DiagnosticSolutionsController(IDiagnosticSolutionRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiagnosticSolutionListDto>>> GetAll()
        {
            var list = await _repo.GetAllAsync();
            if (!list.Any()) return NoContent();
            return Ok(_mapper.Map<List<DiagnosticSolutionListDto>>(list));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiagnosticSolutionGetDto>> GetById(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound(new { message = "Solution not found" });
            return Ok(_mapper.Map<DiagnosticSolutionGetDto>(entity));
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<DiagnosticSolutionListDto>>> GetByStatus(string status)
        {
            var list = await _repo.GetByStatusAsync(status);
            return Ok(_mapper.Map<List<DiagnosticSolutionListDto>>(list));
        }

        [HttpGet("developer/{devId}")]
        public async Task<ActionResult<IEnumerable<DiagnosticSolutionListDto>>> GetByDeveloper(int devId)
        {
            var list = await _repo.GetByDeveloperAsync(devId);
            return Ok(_mapper.Map<List<DiagnosticSolutionListDto>>(list));
        }

        [HttpPost]
        public async Task<ActionResult<DiagnosticSolutionGetDto>> Create(DiagnosticSolutionCreateDto dto)
        {
            var entity = new DiagnosticSolution
            {
                Description = dto.Description,
                HexFileContent = dto.HexFileContent,
                HexFileName = dto.HexFileName,
                Version = dto.Version,
                Status = dto.Status,
                DiagnosticId = dto.DiagnosticId,
                DeveloperId = dto.DeveloperId,
                CreatedAt = DateTime.UtcNow
            };

            var added = await _repo.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = added.Id }, _mapper.Map<DiagnosticSolutionGetDto>(added));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DiagnosticSolutionGetDto>> Update(int id, DiagnosticSolutionUpdateDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            var updated = await _repo.UpdateAsync(entity);
            return Ok(_mapper.Map<DiagnosticSolutionGetDto>(updated));
        }


        [HttpPost("{id}/approve")]
        public async Task<ActionResult<DiagnosticSolutionGetDto>> ApproveMessage(int id, [FromBody] DiagnosticSolutionApproveDto dto)
        {
            try
            {
                var message = await _repo.ApproveAsync(id, dto.ApprovedByDeveloperId);
                if (message == null)
                    return NotFound(new { message = $"Message with ID {id} not found or not in Pending status" });

                var messageDto = _mapper.Map<DiagnosticSolutionGetDto>(message);
                return Ok(messageDto);
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


        //[HttpPost("{id}/reject")]
        //public async Task<ActionResult<DiagnosticSolutionGetDto>> Reject(int id, DiagnosticSolutionRejectDto dto)
        //{
        //    var result = await _repo.RejectAsync(id, dto.RejectionReason);
        //    return result == null ? NotFound() : Ok(_mapper.Map<DiagnosticSolutionGetDto>(result));
        //}


        // POST: api/BaseMessages/5/reject
        [HttpPost("{id}/reject")]
        public async Task<ActionResult<DiagnosticSolutionGetDto>> Reject(int id, [FromBody] DiagnosticSolutionRejectDto dto)
        {
            try
            {
                var message = await _repo.RejectAsync(id, dto.RejectionReason);
                if (message == null)
                    return NotFound(new { message = $"Message with ID {id} not found or not in Pending status" });

                var messageDto = _mapper.Map<DiagnosticSolutionGetDto>(message);
                return Ok(messageDto);
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



            [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();
            await _repo.UpdateAsync(entity);
            return NoContent();
        }
    }
}
