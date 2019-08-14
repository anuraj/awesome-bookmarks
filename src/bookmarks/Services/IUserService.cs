namespace bookmarks.Services
{
    public interface IUserService
    {
        bool IsAuthenticated { get; }
        string Id { get; }
        string Name { get; }
    }
