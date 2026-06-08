using Snackis.Domain.Entities;
using Snackis.Domain.Interface;


namespace Snackis.Application.Services;

public class PrivateMessageService : IPrivateMessageService
{
    private readonly IPrivateMessageRepository _privateMessageRepository;

    public PrivateMessageService(IPrivateMessageRepository privateMessageRepository)
    {
        _privateMessageRepository = privateMessageRepository;
    }

    public async Task<List<PrivateMessage>> GetConversationAsync(string userId1, string userId2) =>
        await _privateMessageRepository.GetConversationAsync(userId1, userId2);

    public async Task<List<PrivateMessage>> GetInboxAsync(string userId) =>
        await _privateMessageRepository.GetInboxAsync(userId);

    public async Task SendAsync(string senderId, string receiverId, string content)
    {
        var message = new PrivateMessage
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = content,
            SentAt = DateTime.Now
        };
        await _privateMessageRepository.CreateAsync(message);
    }


    public async Task DeleteAsync(int id, string requestingUserId)
    {
        var message = await _privateMessageRepository.GetOneAsync(id);
        if (message != null && (message.SenderId == requestingUserId || message.ReceiverId == requestingUserId))
        {
            await _privateMessageRepository.DeleteAsync(message);
        }
    }
}





