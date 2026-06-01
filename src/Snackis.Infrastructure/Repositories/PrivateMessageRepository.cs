using Microsoft.EntityFrameworkCore;
using Snackis.Domain.Entities;
using Snackis.Domain.Interface;
using Snackis.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snackis.Infrastructure.Repositories
{
    public class PrivateMessageRepository : IPrivateMessageRepository
    {
        private readonly SnackisDbContext _db;

        public PrivateMessageRepository(SnackisDbContext db)
        {
            _db = db;
        }


        public async Task<List<PrivateMessage>> GetConversationAsync(string userId1, string userId2) =>
           await _db.PrivateMessages
               .Where(m =>
                   (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                   (m.SenderId == userId2 && m.ReceiverId == userId1))
               .OrderBy(m => m.SentAt)
               .ToListAsync();

        public async Task<List<PrivateMessage>> GetInboxAsync(string userId) =>
          await _db.PrivateMessages
              .Where(m => m.ReceiverId == userId || m.SenderId == userId)
              .OrderByDescending(m => m.SentAt)
              .ToListAsync();


        public async Task<PrivateMessage?> GetOneAsync(int id) =>
            await _db.PrivateMessages.FirstOrDefaultAsync(m => m.Id == id);

        public async Task CreateAsync(PrivateMessage message)
        {
            _db.PrivateMessages.Add(message);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(PrivateMessage message)
        {
            _db.PrivateMessages.Remove(message);
            await _db.SaveChangesAsync();
        }




    }
}
