using System.ComponentModel.DataAnnotations;

namespace CyberQuiz.DAL.Entities;

// One subcategory contains many questions
public class SubCategory
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty; 

    public int CategoryId { get; set; } // FK to the related category
    public int SortOrder { get; set; }  // For UnlockSystem: ordering levels 1, 2, 3 for the system knows what will unlock next, etc.
    public Category Category { get; set; } = null!; //Navigation property for the related category

    public ICollection<Question> Questions { get; set; } = new List<Question>();  //Navigation property for related questions
}