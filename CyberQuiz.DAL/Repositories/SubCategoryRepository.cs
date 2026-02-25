using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Entities;
using CyberQuiz.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CyberQuiz.DAL.Repositories;

public class SubCategoryRepository : ISubCategoryRepository
{
    private readonly CyberQuizDbContext _db;

    public SubCategoryRepository(CyberQuizDbContext db)
    {
        _db = db;
    }

    public async Task<List<SubCategory>> GetAllAsync()
        => await _db.SubCategories.ToListAsync();

    public async Task<SubCategory?> GetByIdAsync(int id)
        => await _db.SubCategories.FindAsync(id);

    public async Task AddAsync(SubCategory subCategory)
        => await _db.SubCategories.AddAsync(subCategory);

    public void Remove(SubCategory subCategory)
        => _db.SubCategories.Remove(subCategory);
}