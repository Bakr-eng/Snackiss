
using Snackis.Domain.Entities;
using Snackis.Domain.Interface;

namespace Snackis.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<Category>> GetAllAsync() =>
        await _categoryRepository.GetAllAsync();

    public async Task CreateAsync(string name, int? parentId)
    {
        var category = new Category { Name = name, ParentCategoryId = parentId };
        await _categoryRepository.CreateAsync(category);
    }

    public async Task UpdateAsync(int id, string name)
    {
        var category = await _categoryRepository.GetOneAsync(id);
        if (category != null)
        {
            category.Name = name;
            await _categoryRepository.UpdateAsync(category);
        }
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _categoryRepository.GetOneAsync(id);
        if (category != null)
        {
            await _categoryRepository.DeleteAsync(category);
        }
    }
}
