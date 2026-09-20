using AutoMapper;
using ConferenceRooms.Api.DTOs.BookingDtos;
using ConferenceRooms.Api.DTOs.RoomDtos;
using ConferenceRooms.DAL.Entities;

namespace ConferenceRooms.Api.Mappings;
/// <summary>
/// Профіль конфігурації AutoMapper для визначення правил перетворення між сутностями DAL та API DTO.
/// </summary
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Мапінг із DTO у сутність бази даних (для створення)
        CreateMap<CreateRoomDto, Room>();

        // Мапінг із сутності бази даних у DTO (якщо потрібно повертати клієнту)
        CreateMap<Room, RoomResponseDto>();
        
        // Мапінг для бронювань
        CreateMap<Booking, BookingResponseDto>();
    }
}