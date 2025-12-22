

//using AutoMapper;
//using Fota.Data;
//using Fota.Models;
//using Fota.Models.DTOs;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace Fota.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class Publishers : ControllerBase
//    {
//        private readonly AppDbContext _context;
//        private readonly IMapper _mapper;

//        public Publishers(AppDbContext context, IMapper mapper)
//        {
//            _context = context;
//            _mapper = mapper;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<PublisherGetDTO>>> GetPublishers()
//        {
//            var publishers = await _context.Publishers.ToListAsync();
//            return Ok(_mapper.Map<List<PublisherGetDTO>>(publishers));
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<PublisherGetDTO>> GetPublisher(int id)
//        {
//            var publisher = await _context.Publishers.FindAsync(id);
//            if (publisher == null)
//                return NotFound();

//            return Ok(_mapper.Map<PublisherGetDTO>(publisher));
//        }

//        [HttpPost]
//        public async Task<ActionResult<PublisherGetDTO>> CreatePublisher(PublisherPostDTO dto)
//        {
//            var entity = _mapper.Map<Publisher>(dto);

//            _context.Publishers.Add(entity);
//            await _context.SaveChangesAsync();

//            var result = _mapper.Map<PublisherPostDTO>(entity);

//            return CreatedAtAction(nameof(GetPublisher), new { id = entity.PublisherId }, result);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdatePublisher(int id, PublisherPostDTO dto)
//        {
//            var entity = await _context.Publishers.FindAsync(id);
//            if (entity == null)
//                return NotFound();

//            if (id != entity.PublisherId)
//                return BadRequest();

//            _mapper.Map(dto, entity);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeletePublisher(int id)
//        {
//            var publisher = await _context.Publishers.FindAsync(id);
//            if (publisher == null)
//                return NotFound();

//            _context.Publishers.Remove(publisher);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }
//    }
//}
