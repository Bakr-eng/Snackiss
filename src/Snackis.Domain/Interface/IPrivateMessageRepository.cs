using Snackis.Domain.Entities;

namespace Snackis.Domain.Interface
{
    public interface IPrivateMessageRepository
    {
        Task<List<PrivateMessage>> GetConversationAsync(string userId1, string userId2);
        Task<List<PrivateMessage>> GetInboxAsync(string userId);
        Task<PrivateMessage?> GetOneAsync(int id);
        Task CreateAsync(PrivateMessage message);
        Task DeleteAsync(PrivateMessage message);
    }
}