using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Snackis.Domain.Entities;
using Snackis.Domain.Interface;
using Snackis.Infrastructure.Data;

namespace Snackis.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly SnackisDbContext _db;

        public PostRepository(SnackisDbContext db)
        {
            _db = db;
        }

        public async Task<List<Post>> GetAllAsync() =>
            await _db.Posts.Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt).ToListAsync();

        public async Task<Post?> GetOneAsync(int id) =>
            await _db.Posts.Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<Post>> GetByCategoryAsync(int categoryId) =>
            await _db.Posts.Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .OrderByDescending(p => p.CreatedAt).ToListAsync();

        public async Task CreateAsync(Post post)
        {
            _db.Posts.Add(post);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Post post)
        {
            _db.Posts.Update(post);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Post post)
        {
            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();
        }
    }
}