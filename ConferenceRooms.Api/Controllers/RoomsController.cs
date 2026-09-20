using AutoMapper;
using ConferenceRooms.Api.DTOs.RoomDtos;
using ConferenceRooms.BLL.Services.Interfaces;
using ConferenceRooms.DAL.Entities;
using ConferenceRooms.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRooms.Api.Controllers;
/// <summary>
/// Контролер для управління конференц-залами (CRUD операції та пошук вільних залів).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoomsController(IRoomService roomService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _roomService = roomService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    //  Додавання конференц-залу.
    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDto dto)
    {
        // Використовуємо AutoMapper для перетворення DTO у сутність DAL
        var room = _mapper.Map<Room>(dto);

        await _unitOfWork.Rooms.AddAsync(room);
        await _unitOfWork.CompleteAsync();

        return Ok(new 
        { 
            Message = "Конференц-зал успішно створено.", 
            RoomId = room.Id 
        });
    }
    
    // Редагування інформації про зал.
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoom(int id, [FromBody] UpdateRoomDto dto)
    {
        // Важливо отримати зал одразу з послугами 
        var room = await _unitOfWork.Rooms.GetRoomWithServicesAsync(id);
        if (room == null)
        {
            return NotFound(new { Error = "Конференц-зал із таким ID не знайдено." });
        }

        // Оновлюємо базові поля, якщо вони передані
        room.Name = dto.Name ?? room.Name;
        room.Capacity = dto.Capacity > 0 ? dto.Capacity : room.Capacity;
        room.BasePricePerHour = dto.BasePricePerHour > 0 ? dto.BasePricePerHour : room.BasePricePerHour;

        // Якщо передано новий список послуг, оновлюємо зв'язки
        if (dto.ServiceIds != null)
        {
            // Знаходимо самі сутності послуг за їхніми ID
            var services = await _unitOfWork.Services.GetAllAsync(); // або спеціальний метод GetByIdsAsync
            var selectedServices = services.Where(s => dto.ServiceIds.Contains(s.Id)).ToList();

            // Очищаємо старі і додаємо нові послуги до залу
            room.RoomServices.Clear();
            foreach (var serviceId in dto.ServiceIds)
            {
                var roomService = new RoomService
                {
                    RoomId = room.Id,
                    ServiceId = serviceId
                };
                room.RoomServices.Add(roomService);
            }
        }

        _unitOfWork.Rooms.Update(room);
        await _unitOfWork.CompleteAsync();

        return Ok(new { Message = "Інформацію про зал та послуги успішно оновлено." });
    }
    
        // Видалення конференц-залу.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound(new { Error = "Конференц-зал із таким ID не знайдено." });
            }

            _unitOfWork.Rooms.Delete(room);
            await _unitOfWork.CompleteAsync();

            return Ok(new { Message = "Конференц-зал успішно видалено." });
        }
        
        // Пошук доступних залів за датою, часом та місткістю.
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableRooms(
            [FromQuery] DateTime startTime,
            [FromQuery] DateTime endTime,
            [FromQuery] int requiredCapacity)
        {
            try
            {
                var availableRooms = await _roomService.GetAvailableRoomsAsync(startTime, endTime, requiredCapacity);

                // Мапимо список сутностей у список RoomResponseDto через AutoMapper
                var responseDtos = _mapper.Map<IEnumerable<RoomResponseDto>>(availableRooms);

                return Ok(responseDtos);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }