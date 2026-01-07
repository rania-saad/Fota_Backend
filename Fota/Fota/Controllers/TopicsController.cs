using AutoMapper;
using Fota.BusinessLayer.Interfaces;
using Fota.BusinessLayer.Repositories;
using Fota.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedProjectDTOs.Admin;
using SharedProjectDTOs.Topics;

namespace Fota.Controllers
{
    //[Authorize(Roles = "ADMIN,DEVELOPER")]

    [ApiController]
    [Route("api/[controller]")]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IMapper _mapper;

        public TopicsController(ITopicRepository topicRepository , IMapper mapper)
        {
            _topicRepository = topicRepository;
            _mapper = mapper;
        }

        // GET: api/Topics

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TopicGetDTO>>> GetAllTopics()
        {

            try
            {
                var Topics = await _topicRepository.GetAllAsync();

                if (Topics == null || !Topics.Any())
                {
                    return NoContent(); // Not found
                }

                var TopicDTOs = _mapper.Map<List<TopicGetDTO>>(Topics);
                return Ok(TopicDTOs);
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




        // GET: api/Topics/5

        [HttpGet("{id}")]
        public async Task<ActionResult<TopicGetDTO>> GetTopicById(int id)
        {


            try
            {
                var Topics = await _topicRepository.GetByIdAsync(id);

                if (Topics == null)
                {
                    return NotFound(new { message = $"Topic with ID {id} not found" });
                }


                var TopicDTOs = _mapper.Map<TopicGetDTO>(Topics);
                return Ok(TopicDTOs);
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



        // GET: api/Topics/search?name=Battery
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TopicGetDTO>>> SearchTopics([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Search name cannot be empty" });


            try
            {
                var Topics = await _topicRepository.SearchByNameAsync(name);

                if (Topics == null)
                {
                    return NotFound(new { message = $"Topic with ID {name} not found" });
                }


                var TopicDTOs = _mapper.Map<List<TopicGetDTO>>(Topics);
                return Ok(TopicDTOs);
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

        // GET: api/Topics/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Topic>>> GetActiveTopics()
        {

            try
            {
                var Topics = await _topicRepository.GetActiveTopicsAsync();

                if (Topics == null)
                {
                    return NotFound(new { message = $"Active Topics  not found" });
                }


                var TopicDTOs = _mapper.Map<List<TopicGetDTO>>(Topics);
                return Ok(TopicDTOs);
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

        // GET: api/Topics/team/1
        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<IEnumerable<Topic>>> GetTopicsByTeam(int teamId)
        {
            var topics = await _topicRepository.GetByTeamAsync(teamId);
            return Ok(topics);
        }

        // POST: api/Topics
        [HttpPost]
        public async Task<ActionResult<Topic>> CreateTopic([FromBody] TopicCreateDto dto)
        {
            var topic = new Topic
            {
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _topicRepository.AddAsync(topic);
            return CreatedAtAction(nameof(GetTopicById), new { id = created.Id }, created);
        }

        // PUT: api/Topics/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Topic>> UpdateTopic(int id, [FromBody] TopicUpdateDto dto)
        {
            var topic = await _topicRepository.GetByIdAsync(id);
            if (topic == null)
                return NotFound(new { message = $"Topic with ID {id} not found" });

            topic.Name = dto.Name;
            topic.Description = dto.Description;
            topic.IsActive = dto.IsActive;
            topic.UpdatedAt = DateTime.UtcNow;

            var updated = await _topicRepository.UpdateAsync(topic);
            return Ok(updated);
        }

        // DELETE: api/Topics/5 (Soft Delete)
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTopic(int id)
        {
            var deleted = await _topicRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Topic with ID {id} not found" });

            return NoContent();
        }
    }
}

// ==================== DEPENDENCY INJECTION SETUP ====================
// 