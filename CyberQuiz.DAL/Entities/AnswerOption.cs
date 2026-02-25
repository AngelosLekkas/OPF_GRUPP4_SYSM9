namespace CyberQuiz.DAL.Entities;

// One anwerOption: many ressults
public class AnswerOption
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }

    public int QuestionId { get; set; }

    //Navigation property for the related question
    public Question Question { get; set; }

    //public ICollection<UserResult> UserResults { get; set; } = new List<UserResult>();
}