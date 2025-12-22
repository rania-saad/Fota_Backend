//using Fota.Data;
//using Fota.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Fota.Models.DTOs;
//using AutoMapper;
//using Fota.Services;
//namespace Fota.Controllers;


//[Route("api/[controller]")]
//[ApiController]
//public class Messages : ControllerBase
//{
//    private readonly AppDbContext _context;
//    private readonly IMapper _mapper;

//    public Messages(AppDbContext context, IMapper mapper)
//    {
//        _context = context;
//        _mapper = mapper;
//    }

//    [HttpGet]
//    public async Task<ActionResult<IEnumerable<MessageGetDTO>>> GetMessages()
//    {
//        var messages = await _context.Messages
//            .ToListAsync();

//        return Ok(_mapper.Map<List<MessageGetDTO>>(messages));
//    }




//    //[HttpPost("send")]
//    //public async Task<ActionResult<MessagePostDTO>> SendMessage(
//    //[FromBody] MessagePostDTO dto,
//    //[FromServices] MqttService mqtt)
//    //{
//    //    dto.CreatedAt = DateTime.UtcNow;

//    //    var entity = _mapper.Map<Message>(dto);
//    //    _context.Messages.Add(entity);
//    //    await _context.SaveChangesAsync();

//    //    // 🟢 هنا نحمّل الـ Topic عشان Topic.Name يبقى موجود
//    //    await _context.Entry(entity)
//    //                  .Reference(m => m.Topic)
//    //                  .LoadAsync();

//    //    // ابعتها للـ MQTT
//    //    await mqtt.PublishAsync(dto.TopicName, dto.Payload);

//    //    var result = _mapper.Map<MessagePostDTO>(entity);
//    //    return Ok(result);
//    //}
//    [HttpPost("send")]
//    public async Task<ActionResult<MessagePostDTO>> SendMessage(
//    [FromBody] MessagePostDTO dto,
//    [FromServices] MqttService mqtt)
//    {
//        dto.CreatedAt = DateTime.UtcNow;

//        // 1️⃣ التأكد من وجود Topic أو إضافته
//        var topic = await _context.Topics.FirstOrDefaultAsync(t => t.Name == dto.TopicName);
//        if (topic == null)
//        {
//            topic = new Topic { Name = dto.TopicName };
//            _context.Topics.Add(topic);
//            await _context.SaveChangesAsync(); // نحفظ عشان ناخد TopicId
//        }

//        // 2️⃣ نربط الرسالة بالـ TopicId
//        var entity = _mapper.Map<Message>(dto);
//        entity.TopicId = topic.TopicId;

//        _context.Messages.Add(entity);
//        await _context.SaveChangesAsync();

//        // 3️⃣ إرسال للـ MQTT
//        await mqtt.PublishAsync(dto.TopicName, dto.Payload);

//        var result = _mapper.Map<MessagePostDTO>(entity);
//        return Ok(result);
//    }



//    [HttpGet("receive/{topicId}")]
//    public async Task<ActionResult<IEnumerable<MessageGetDTO>>> ReceiveMessages(int topicId)
//    {
//        var messages = await _context.Messages
//            .Where(m => m.TopicId == topicId)
//            .OrderByDescending(m => m.CreatedAt)
//            .ToListAsync();

//        if (!messages.Any())
//            return NotFound("No messages found for this topic.");

//        return Ok(_mapper.Map<List<MessageGetDTO>>(messages));
//    }
//}