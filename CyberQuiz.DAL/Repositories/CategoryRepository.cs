using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Entities;
using CyberQuiz.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CyberQuiz.DAL.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly CyberQuizDbContext _db;

    public CategoryRepository(CyberQuizDbContext db)
    {
        _db = db;
    }

    public async Task<List<Category>> GetAllAsync()
        => await _db.Categories.ToListAsync();

    public async Task<Category?> GetByIdAsync(int id)
        => await _db.Categories.FindAsync(id);

    public async Task AddAsync(Category category)
        => await _db.Categories.AddAsync(category);

    public void Remove(Category category)
        => _db.Categories.Remove(category);
}