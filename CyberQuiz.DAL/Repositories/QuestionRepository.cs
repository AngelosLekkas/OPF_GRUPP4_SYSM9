using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Entities;
using CyberQuiz.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CyberQuiz.DAL.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly CyberQuizDbContext _db;

    public QuestionRepository(CyberQuizDbContext db)
    {
        _db = db;
    }

    public async Task<List<Question>> GetAllAsync()
        => await _db.Questions.ToListAsync();

    public async Task<Question?> GetByIdAsync(int id)
        => await _db.Questions.FindAsync(id);

    public async Task AddAsync(Question question)
        => await _db.Questions.AddAsync(question);

    public void Remove(Question question)
        => _db.Questions.Remove(question);
}