namespace CyberQuiz.DAL.Entities;

// one category can have many subcategories, but a subcategory belongs to only one category
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    //public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>(); //Navigation property for related subcategories
}
