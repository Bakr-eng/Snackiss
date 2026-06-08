using Snackis.Domain.Entities;


namespace Snackis.Application.Services;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();
    Task CreateAsync(string name, int? parentId);
    Task UpdateAsync(int id, string name);
    Task DeleteAsync(int id);
}
