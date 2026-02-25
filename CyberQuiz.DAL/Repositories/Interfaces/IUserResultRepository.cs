using CyberQuiz.DAL.Entities;

namespace CyberQuiz.DAL.Repositories.Interfaces;

public interface IUserResultRepository
{
    Task<List<UserResult>> GetAllAsync();
    Task<UserResult?> GetByIdAsync(int id);
    Task AddAsync(UserResult result);
    void Remove(UserResult result);
}