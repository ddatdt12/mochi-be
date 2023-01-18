using MochiApi.Models;

namespace MochiApi.Services
{
    public interface IUserService
    {
        Task<User?> GetById(Guid id);
    }
}
