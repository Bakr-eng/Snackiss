using Snackis.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snackis.Application.Services;

public interface IPostService
{
    Task<List<Post>> GetAllAsync();
    Task<Post?> GetByIdAsync(int id);
    Task<List<Post>> GetByCategoryAsync(int categoryId);
    Task CreateAsync(string title, string content, int categoryId, string userId, string? imageUrl = null);
    Task UpdateAsync(int id, string title, string content);
    Task DeleteAsync(int id);
    Task<AppUser?> GetAuthorAsync(string userId);
}
