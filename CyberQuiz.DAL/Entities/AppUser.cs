using Microsoft.AspNetCore.Identity;

namespace CyberQuiz.DAL.Entities;

// One user : many results
public class AppUser : IdentityUser
{
    //public ICollection<UserResult> Results { get; set; } = new List<UserResult>();
}
