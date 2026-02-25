namespace CyberQuiz.DAL.Entities;

public class UserResult
{
    public int Id { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }

    public int QuestionId { get; set; }
    public Question Question { get; set; }

    public int AnswerOptionId { get; set; }
    public AnswerOption AnswerOption { get; set; }

    public bool IsCorrect { get; set; }
}
