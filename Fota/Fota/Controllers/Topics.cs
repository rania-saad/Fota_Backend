//using AutoMapper;
//using Fota.Data;
//using Fota.Models;
//using Fota.Models.DTOs;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Fota.MappingProfiles;

//namespace Fota.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class Topics : ControllerBase
//    {

//        private readonly AppDbContext _context;
//        private readonly IMapper _mapper;


//        public Topics(AppDbContext context, IMapper mapper)
//        {
//            _context = context;
//            _mapper = mapper;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Topic>>> GetTopics()
//        {
//            return await _context.Topics.ToListAsync();
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<Topic>> GetTopic(int id)
//        {
//            var topic = await _context.Topics.FindAsync(id);
//            if (topic == null) return NotFound();
//            return topic;
//        }

//        [HttpPost]
//        public async Task<ActionResult<TopicDTO>> CreateTopic(TopicDTO topic)
//        {
//            var entity = _mapper.Map<Topic>(topic);
//            _context.Topics.Add(entity);
//            await _context.SaveChangesAsync();
//            var result = _mapper.Map<TopicDTO>(entity);

//            return Ok(result);
//        }
//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateTopic(int id, TopicDTO topicDto)
//        {
//            var entity = await _context.Topics.FindAsync(id);
//            if (entity == null) return NotFound();

//            // عدّل القيم باستخدام AutoMapper
//            _mapper.Map(topicDto, entity);

//            await _context.SaveChangesAsync();
//            return NoContent();
//        }


//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteTopic(int id)
//        {
//            var topic = await _context.Topics.FindAsync(id);
//            if (topic == null) return NotFound();
//            _context.Topics.Remove(topic);
//            await _context.SaveChangesAsync();
//            return NoContent();
//        }
//    }
//}
