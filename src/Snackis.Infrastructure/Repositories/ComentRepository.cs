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
    public class ComentRepository : IComentRepository
    {
        private readonly SnackisDbContext _db;

        public ComentRepository(SnackisDbContext db)
        {
            _db = db;
        }


        public async Task<List<Coment>> GetAllAsync() =>
            await _db.Coments
            .Include(c => c.Post)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        public async Task<List<Coment>> GetByPostAsync(int postId) =>
            await _db.Coments
            .Where(c => c.PostId == postId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();

        public async Task<Coment?> GetOneAsync(int id) =>
            await _db.Coments
            .Include(c => c.Post)
            .FirstOrDefaultAsync(c => c.Id == id);


        public async Task CreateAsync(Coment coment)
        {
            _db.Coments.Add(coment);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Coment coment)
        {
            _db.Coments.Update(coment);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteAsync(Coment coment)
        {
            _db.Coments.Remove(coment);
            await _db.SaveChangesAsync();
        }

      
    }
}
