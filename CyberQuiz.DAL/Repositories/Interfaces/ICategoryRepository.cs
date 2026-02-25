using CyberQuiz.DAL.Entities;

namespace CyberQuiz.DAL.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task AddAsync(Category category);
    void Remove(Category category);
}