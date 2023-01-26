using MochiApi.Dtos;
using MochiApi.Models;

namespace MochiApi.Services
{
    public interface IEventService
    {
        Task<Event> DeleteEvent(int userId, int eventId);
        Task<Event> ToggleEventFinish(int userId, int eventId);
    }
}
