using Microsoft.AspNetCore.Identity;

namespace CyberQuiz.DAL.Entities;

// One user : many results
public class AppUser : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true; // If account is active or deactivated (soft delete)
    public int TotalQuizzesTaken { get; set; } // How many quizzes the user has taken
    public int HighestScore { get; set; }  // The highest score the user has achieved across all quizzes
    public string? ProfilePictureUrl { get; set; } // URL to the user's profile picture

    public ICollection<UserResult> Results { get; set; } = new List<UserResult>();
}
