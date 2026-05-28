using Snackis.Domain.Entities;
using Snackis.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snackis.Application.Services
{
    public class ComentService : IComentService
    {
        private readonly IComentRepository _comentRepository;

        public ComentService(IComentRepository comentRepository)
        {
            _comentRepository = comentRepository;
        }

        public async Task<List<Coment>> GetByPostAsync(int postId) =>
           await _comentRepository.GetByPostAsync(postId);

        public async Task CreateAsync(string content, int postId, string userId)
        {
            var coment = new Coment
            {
                Content = content,
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.Now
            };
            await _comentRepository.CreateAsync(coment);
        }

        public async Task DeleteAsync(int id)
        {
            var coment = await _comentRepository.GetOneAsync(id);
            if (coment != null)
            {
                await _comentRepository.DeleteAsync(coment);
            }
        }
    }
}