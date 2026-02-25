namespace CyberQuiz.DAL.Entities;

// One question : many answer options
public class Question
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;

    public int SubCategoryId { get; set; }

    //Navigation property for the related subcategory
    public SubCategory SubCategory { get; set; }

    //public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
    //public ICollection<UserResult> Results { get; set; } = new List<UserResult>();
}