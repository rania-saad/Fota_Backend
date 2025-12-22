//using AutoMapper;
//using Fota.Data;
//using Fota.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Fota.Models.DTOs;
//using Fota.MappingProfiles;

//namespace Fota.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class Subscribers : ControllerBase
//    {
//        private readonly AppDbContext _context;
//        private readonly IMapper _mapper;


//        public Subscribers(AppDbContext context, IMapper mapper)
//        {
//            _context = context;
//            _mapper = mapper;
//        }




//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Subscriber>>> GetSubscribers()
//        {
//            return await _context.Subscribers.ToListAsync();
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<Subscriber>> GetSubscriber(int id)
//        {
//            var subscriber = await _context.Subscribers.FindAsync(id);
//            if (subscriber == null) return NotFound();
//            return subscriber;
//        }

//        [HttpPost]
//        public async Task<ActionResult<SubscriberDTO>> CreateSubscriber(SubscriberDTO subscriber)
//        {
//            var entity = _mapper.Map<Subscriber>(subscriber);
//            _context.Subscribers.Add(entity);
//            await _context.SaveChangesAsync();
//            var result = _mapper.Map<SubscriberDTO>(entity);
//            return Ok(result);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateSubscriber(int id, SubscriberDTO subscriber)
//        {
//            var entity = await _context.Subscribers.FindAsync(id);

//            if (id != entity.SubscriberId) return BadRequest();
//            _mapper.Map(subscriber, entity);
//            await _context.SaveChangesAsync();
//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteSubscriber(int id)
//        {
//            var subscriber = await _context.Subscribers.FindAsync(id);
//            if (subscriber == null) return NotFound();
//            _context.Subscribers.Remove(subscriber);
//            await _context.SaveChangesAsync();
//            return NoContent();
//        }
//    }
//}

