using CyberQuiz.DAL.Entities;

namespace CyberQuiz.DAL.Repositories.Interfaces;

public interface ISubCategoryRepository
{
    Task<List<SubCategory>> GetAllAsync();
    Task<SubCategory?> GetByIdAsync(int id);
    Task AddAsync(SubCategory subCategory);
    void Remove(SubCategory subCategory);
}
