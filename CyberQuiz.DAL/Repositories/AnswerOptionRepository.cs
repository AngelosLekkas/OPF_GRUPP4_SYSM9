using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Entities;
using CyberQuiz.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CyberQuiz.DAL.Repositories;

public class AnswerOptionRepository : IAnswerOptionRepository
{
    private readonly CyberQuizDbContext _db;

    public AnswerOptionRepository(CyberQuizDbContext db)
    {
        _db = db;
    }

    public async Task<List<AnswerOption>> GetAllAsync()
        => await _db.AnswerOptions.ToListAsync();

    public async Task<AnswerOption?> GetByIdAsync(int id)
        => await _db.AnswerOptions.FindAsync(id);

    public async Task AddAsync(AnswerOption option)
        => await _db.AnswerOptions.AddAsync(option);

    public void Remove(AnswerOption option)
        => _db.AnswerOptions.Remove(option);
}
