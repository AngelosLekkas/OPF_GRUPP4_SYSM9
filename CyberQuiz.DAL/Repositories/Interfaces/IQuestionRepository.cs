using CyberQuiz.DAL.Entities;

namespace CyberQuiz.DAL.Repositories.Interfaces;

public interface IQuestionRepository
{

    // Returns all questions from the database
    Task<List<Question>> GetAllAsync(CancellationToken cancellationToken = default);

    // Returns 1 question with given id, or null if not found
    Task<Question?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    // Returns 1 question with answer options included
    Task<Question?> GetByIdWithAnswerOptionsAsync(int id, CancellationToken cancellationToken = default);

    // Returns all questions that belong to given subCategoryId
    Task<List<Question>> GetBySubCategoryAsync(int subCategoryId, CancellationToken cancellationToken = default);

    // Returns all questions in a subcategory including answer options
    Task<List<Question>> GetBySubCategoryWithAnswerOptionsAsync(int subCategoryId, CancellationToken cancellationToken = default);

    // Returns only question ids for a given subcategory to support progress queries
    Task<List<int>> GetIdsBySubCategoryAsync(int subCategoryId, CancellationToken cancellationToken = default);
    
    // Returns counts of questions grouped by SubCategoryId for given sub ids
    Task<Dictionary<int,int>> GetQuestionCountsBySubIdsAsync(IEnumerable<int> subIds, CancellationToken cancellationToken = default);
    
    Task AddAsync(Question question);
   
    void Remove(Question question);
}