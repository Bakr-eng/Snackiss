using Snackis.Domain.Entities;

namespace Snackis.Application.Services;

public interface IPrivateMessageService
{
    Task<List<PrivateMessage>> GetConversationAsync(string userId1, string userId2);
    Task<List<PrivateMessage>> GetInboxAsync(string userId);
    Task SendAsync(string senderId, string receiverId, string content);
    Task DeleteAsync(int id, string requestingUserId);
}