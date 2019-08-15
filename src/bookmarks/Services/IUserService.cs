using bookmarks.Models;

namespace bookmarks.Services
{
    public interface IUserService
    {
        bool IsAuthenticated { get; }
        string Id { get; }
        string Name { get; }
        string UPN { get; }
    }
}