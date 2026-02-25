using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Entities;
using CyberQuiz.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CyberQuiz.DAL.Repositories;

public class UserResultRepository : IUserResultRepository
{
    private readonly CyberQuizDbContext _db;

    public UserResultRepository(CyberQuizDbContext db)
    {
        _db = db;
    }

    public async Task<List<UserResult>> GetAllAsync()
        => await _db.UserResults.ToListAsync();

    public async Task<UserResult?> GetByIdAsync(int id)
        => await _db.UserResults.FindAsync(id);

    public async Task AddAsync(UserResult result)
        => await _db.UserResults.AddAsync(result);

    public void Remove(UserResult result)
        => _db.UserResults.Remove(result);
}
