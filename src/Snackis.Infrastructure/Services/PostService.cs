using Microsoft.EntityFrameworkCore;
using Snackis.Application.Service;
using Snackis.Domain.Entities;
using Snackis.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snackis.Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly SnackisDbContext _db;

        public PostService(SnackisDbContext db)
        {
            _db = db;
        }

        public async Task<List<Post>> GetAllAsync() =>
            await _db.Posts
               .Include(p => p.Category)
               .OrderByDescending(p => p.CreatedAt)
               .ToListAsync();


        public async Task<Post?> GetByIdAsync(int id) =>
            await _db.Posts
               .Include(p => p.Category)
               .FirstOrDefaultAsync(p => p.Id == id);


        public async Task<List<Post>> GetByCategoryAsync(int categoryId) =>
            await _db.Posts
              .Include(p => p.Category)
              .Where(p => p.CategoryId == categoryId)
              .OrderByDescending(p => p.CreatedAt)
              .ToListAsync();


        public async Task CreateAsync(string title, string content, int categoryId, string userId)
        {
            _db.Posts.Add(new Post
            {
                Title = title,
                Content = content,
                CategoryId = categoryId,
                UserId = userId,
                CreatedAt = DateTime.Now
            });
            await _db.SaveChangesAsync();
        }


        public async Task UpdateAsync(int id, string title, string content)
        {
            var post = await _db.Posts.FindAsync(id);
            if (post != null)
            {
                post.Title = title;
                post.Content = content;
                await _db.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync(int id)
        {
            var post = await _db.Posts.FindAsync(id);
            if (post != null)
            {
                _db.Posts.Remove(post);
                await _db.SaveChangesAsync();
            }
        }

    }
}
