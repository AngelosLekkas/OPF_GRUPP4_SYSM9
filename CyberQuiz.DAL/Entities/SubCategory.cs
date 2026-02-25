namespace CyberQuiz.DAL.Entities;

// One subcategory contains many questions
public class SubCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public Category Category { get; set; } //Navigation property for the related category

    public ICollection<Question> Questions { get; set; } = new List<Question>();  //Navigation property for related questions
}