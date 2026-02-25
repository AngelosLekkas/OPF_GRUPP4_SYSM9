using CyberQuiz.DAL.Entities;

namespace CyberQuiz.DAL.Repositories.Interfaces;

public interface IAnswerOptionRepository
{
    Task<List<AnswerOption>> GetAllAsync();
    Task<AnswerOption?> GetByIdAsync(int id);
    Task AddAsync(AnswerOption option);
    void Remove(AnswerOption option);
}