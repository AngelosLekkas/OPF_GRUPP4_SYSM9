using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Entities;
using CyberQuiz.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Linq;

namespace CyberQuiz.DAL.Repositories;

public class UserResultRepository : IUserResultRepository
{
    private readonly CyberQuizDbContext _db;

    public UserResultRepository(CyberQuizDbContext db)
    {
        _db = db;
    }


    // Returns all UserResults from the database (AsNoTracking = ReadOnly)
    public async Task<List<UserResult>> GetAllAsync()
        => await _db.UserResults
            .AsNoTracking()
            .ToListAsync();


    // Returns a UserResult by its Id, or null if not found
    public async Task<UserResult?> GetByIdAsync(int id)
        => await _db.UserResults.FindAsync(id);


    // Returns the latest UserResult per QuestionId for a specific user
    public async Task<List<UserResult>> GetLatestResultsForUserAndQuestionIdsAsync(string userId, IEnumerable<int> questionIds, CancellationToken cancellationToken = default)
        => await _db.UserResults
            .AsNoTracking()
            .Where(r => r.UserId == userId && questionIds.Contains(r.QuestionId)) // Filter by userId and questionIds
            .GroupBy(r => r.QuestionId)                                         
            .Select(g => g.OrderByDescending(x => x.Id).First())    // Each group chooses the latest result for a question
            .ToListAsync(cancellationToken);

    // Adds a new UserResult to the database, but does not save changes (SaveChanges is called in UnitOfWork)
    public async Task AddAsync(UserResult result)
        => await _db.UserResults.AddAsync(result);

    // Marks an existing UserResult as modified, so that it will be updated in the database when SaveChanges is called
    public void Remove(UserResult result)
        => _db.UserResults.Remove(result);
}
