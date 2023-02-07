using MochiApi.Dtos;
using MochiApi.Models;

namespace MochiApi.Services
{

    public interface INotiService
    {
        Task<IEnumerable<Notification>> CreateListNoti(IEnumerable<CreateNotificationDto> notiDto, bool saveChanges = false);
        Task<Notification> CreateNoti(CreateNotificationDto notiDto);
        Task<IEnumerable<Notification>> GetNotifications(int userId);
        Task MarkSeen(int id);
        Task MarkSeenAllNotis(int userId);
    }
}
