
using AutoMapper;
using Fota.BusinessLayer.Interfaces;
using Fota.BusinessLayer.Repositories;
using Fota.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedProjectDTOs.Subscribers;

namespace Fota.AWebAPI.Controllers
{
    [Authorize(Roles = "ADMIN,DEVELOPER")]

    [ApiController]
    [Route("api/[controller]")]
    public class SubscribersController : ControllerBase
    {
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IMapper _mapper;

        public SubscribersController(ISubscriberRepository subscriberRepository, IMapper mapper)
        {
            _subscriberRepository = subscriberRepository;
            _mapper = mapper;
        }

        // GET: api/Subscribers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriberGetDto>>> GetAllSubscribers()
        {
            try
            {
                var subscribers = await _subscriberRepository.GetAllAsync();

                if (subscribers == null || !subscribers.Any())
                {
                    return NoContent();
                }

                var subscriberDtos = _mapper.Map<List<SubscriberGetDto>>(subscribers);
                return Ok(subscriberDtos);
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

        // GET: api/Subscribers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriberGetDto>> GetSubscriberById(int id)
        {
            try
            {
                var subscriber = await _subscriberRepository.GetByIdAsync(id);

                if (subscriber == null)
                {
                    return NotFound(new { message = $"Subscriber with ID {id} not found" });
                }

                var subscriberDto = _mapper.Map<SubscriberGetDto>(subscriber);
                return Ok(subscriberDto);
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

        // GET: api/Subscribers/search?name=john
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<SubscriberGetDto>>> SearchSubscribers([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Search name cannot be empty" });

            try
            {
                var subscribers = await _subscriberRepository.SearchByNameAsync(name);
                var subscriberDtos = _mapper.Map<List<SubscriberGetDto>>(subscribers);
                return Ok(subscriberDtos);
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

        // GET: api/Subscribers/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<SubscriberGetDto>>> GetActiveSubscribers()
        {
            try
            {
                var subscribers = await _subscriberRepository.GetActiveSubscribersAsync();
                var subscriberDtos = _mapper.Map<List<SubscriberGetDto>>(subscribers);
                return Ok(subscriberDtos);
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

        // GET: api/Subscribers/topic/5
        [HttpGet("topic/{topicId}")]
        public async Task<ActionResult<IEnumerable<SubscriberTopicDto>>> GetSubscribersByTopic(int topicId)
        {
            try
            {
                var subscribers = await _subscriberRepository.GetByTopicAsync(topicId);
                var subscriberDtos = _mapper.Map<List<SubscriberTopicDto>>(subscribers);
                return Ok(subscriberDtos);
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

        // POST: api/Subscribers
        [HttpPost]
        public async Task<ActionResult<SubscriberGetDto>> CreateSubscriber([FromBody] SubscriberCreateDto dto)
        {
            try
            {
                var existing = await _subscriberRepository.GetByEmailAsync(dto.Email);
                if (existing != null)
                    return Conflict(new { message = "Email already exists" });

                var subscriber = new Subscriber
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var created = await _subscriberRepository.AddAsync(subscriber);
                var subscriberDto = _mapper.Map<SubscriberGetDto>(created);
                return CreatedAtAction(nameof(GetSubscriberById), new { id = created.Id }, subscriberDto);
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

        // PUT: api/Subscribers/5
        [HttpPut("{id}")]
        public async Task<ActionResult<SubscriberGetDto>> UpdateSubscriber(int id, [FromBody] SubscriberUpdateDto dto)
        {
            try
            {
                var subscriber = await _subscriberRepository.GetByIdAsync(id);
                if (subscriber == null)
                    return NotFound(new { message = $"Subscriber with ID {id} not found" });

                subscriber.Name = dto.Name;
                subscriber.PhoneNumber = dto.PhoneNumber;
                subscriber.IsActive = dto.IsActive;
                subscriber.UpdatedAt = DateTime.UtcNow;

                var updated = await _subscriberRepository.UpdateAsync(subscriber);
                var subscriberDto = _mapper.Map<SubscriberGetDto>(updated);
                return Ok(subscriberDto);
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

        // DELETE: api/Subscribers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubscriber(int id)
        {
            try
            {
                var deleted = await _subscriberRepository.DeleteAsync(id);
                if (!deleted)
                    return NotFound(new { message = $"Subscriber with ID {id} not found" });

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


        [HttpGet("TotalSubscribers")]
        public async Task<IActionResult> GetTotalSubscribers()
        {
            var totalMessages = await _subscriberRepository.GetTotalCountAsync();
            return Ok(new { TotalSubscribers = totalMessages });
        }
    }
}