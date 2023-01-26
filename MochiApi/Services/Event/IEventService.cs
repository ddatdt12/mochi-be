using MochiApi.Dtos;
using MochiApi.Models;

namespace MochiApi.Services
{
    public interface IEventService
    {
        Task<Event> CreateEvent(int userId, CreateEventDto createEventDto);
        Task<Event> DeleteEvent(int userId, int eventId);
        Task<Event?> GetEventById(int id, int userId);
        Task<IEnumerable<Event>> GetEvents(int userId);
        Task<IEnumerable<Event>> GetEventsOfWallet(int userId, int walletId);
        Task<IEnumerable<Transaction>> GetTransactionOfEvent(int id);
        Task<Event> ToggleEventFinish(int userId, int eventId);
        Task<Event> UpdateEvent(int userId, int eventId, UpdateEventDto updateDto);
        Task UpdateEventSpent(int eventId);
    }
}
