using CyberQuiz.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CyberQuiz.DAL.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(
        CyberQuizDbContext db,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        await db.Database.MigrateAsync();

        // Seed Roles
        string[] roles = { "User", "Admin" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }


        // Seed default user
        if (!userManager.Users.Any())
        {
            // Create regular user
            var user = new AppUser
            {
                UserName = "user",
                Email = "user@example.com"
            };

            var result = await userManager.CreateAsync(user, "Password1234!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
            }

            // Create admin user
            var admin = new AppUser
            {
                UserName = "admin",
                Email = "admin@cyberquiz.com"
            };

            var adminResult = await userManager.CreateAsync(admin, "Admin1234!");

            if (adminResult.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }



        // Seed Categories
        if (!db.Categories.Any())
        {
            var cat = new Category { Name = "Cyber Security Basics" };

            var sub1 = new SubCategory { Name = "Passwords", Category = cat };
            var sub2 = new SubCategory { Name = "Phishing", Category = cat };
            var sub3 = new SubCategory { Name = "Malware", Category = cat };
            var sub4 = new SubCategory { Name = "Network Security", Category = cat };
            var sub5 = new SubCategory { Name = "Social Engineering", Category = cat };

            var q1 = new Question
            {
                Text = "What is a strong password?",
                SubCategory = sub1,
                AnswerOptions =
                {
                    new AnswerOption { Text = "123456", IsCorrect = false },
                    new AnswerOption { Text = "Long + complex", IsCorrect = true }
                }
            };

            var q2 = new Question
            {
                Text = "How often should you change your password?",
                SubCategory = sub1,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Never", IsCorrect = false },
                    new AnswerOption { Text = "Every year", IsCorrect = false },
                    new AnswerOption { Text = "When compromised or every 3-6 months", IsCorrect = true },
                    new AnswerOption { Text = "Every day", IsCorrect = false }
                }
            };

            var q3 = new Question
            {
                Text = "Should you use the same password for multiple accounts?",
                SubCategory = sub1,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Yes, it's easier to remember", IsCorrect = false },
                    new AnswerOption { Text = "No, use unique passwords", IsCorrect = true },
                    new AnswerOption { Text = "Only for important accounts", IsCorrect = false }
                }
            };

            var q4 = new Question
            {
                Text = "What is a password manager?",
                SubCategory = sub1,
                AnswerOptions =
                {
                    new AnswerOption { Text = "A person who manages passwords", IsCorrect = false },
                    new AnswerOption { Text = "Tool to securely store and generate passwords", IsCorrect = true },
                    new AnswerOption { Text = "A spreadsheet with passwords", IsCorrect = false }
                }
            };

            var q5 = new Question
            {
                Text = "Which makes a password stronger?",
                SubCategory = sub1,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Using only lowercase letters", IsCorrect = false },
                    new AnswerOption { Text = "Using your birthday", IsCorrect = false },
                    new AnswerOption { Text = "Mixing letters, numbers, and symbols", IsCorrect = true },
                    new AnswerOption { Text = "Using common words", IsCorrect = false }
                }
            };

            var q6 = new Question
            {
                Text = "What is phishing?",
                SubCategory = sub2,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Fishing in a lake", IsCorrect = false },
                    new AnswerOption { Text = "Tricking users to reveal info", IsCorrect = true }
                }
            };

            var q7 = new Question
            {
                Text = "How can you identify a phishing email?",
                SubCategory = sub2,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Check sender's email address", IsCorrect = true },
                    new AnswerOption { Text = "Trust all emails from banks", IsCorrect = false },
                    new AnswerOption { Text = "Click all links to verify", IsCorrect = false }
                }
            };

            var q8 = new Question
            {
                Text = "What should you do if you receive a suspicious email?",
                SubCategory = sub2,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Click the link to check", IsCorrect = false },
                    new AnswerOption { Text = "Reply with your password", IsCorrect = false },
                    new AnswerOption { Text = "Delete it and report it", IsCorrect = true },
                    new AnswerOption { Text = "Forward it to everyone", IsCorrect = false }
                }
            };

            var q9 = new Question
            {
                Text = "What is spear phishing?",
                SubCategory = sub2,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Targeted phishing attack on specific individuals", IsCorrect = true },
                    new AnswerOption { Text = "Fishing with a spear", IsCorrect = false },
                    new AnswerOption { Text = "Mass email campaign", IsCorrect = false }
                }
            };

            var q10 = new Question
            {
                Text = "Which is a red flag in a phishing email?",
                SubCategory = sub2,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Urgent action required", IsCorrect = true },
                    new AnswerOption { Text = "Professional formatting", IsCorrect = false },
                    new AnswerOption { Text = "Company logo", IsCorrect = false },
                    new AnswerOption { Text = "Proper grammar", IsCorrect = false }
                }
            };

            var q11 = new Question
            {
                Text = "What is malware?",
                SubCategory = sub3,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Software designed to harm systems", IsCorrect = true },
                    new AnswerOption { Text = "A type of hardware", IsCorrect = false },
                    new AnswerOption { Text = "A security tool", IsCorrect = false }
                }
            };

            var q12 = new Question
            {
                Text = "Which of the following is a type of malware?",
                SubCategory = sub3,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Firewall", IsCorrect = false },
                    new AnswerOption { Text = "Ransomware", IsCorrect = true },
                    new AnswerOption { Text = "Antivirus", IsCorrect = false },
                    new AnswerOption { Text = "Browser", IsCorrect = false }
                }
            };

            var q13 = new Question
            {
                Text = "What is the best protection against malware?",
                SubCategory = sub3,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Antivirus software + safe browsing habits", IsCorrect = true },
                    new AnswerOption { Text = "Clicking on all ads", IsCorrect = false },
                    new AnswerOption { Text = "Disabling firewall", IsCorrect = false }
                }
            };

            var q14 = new Question
            {
                Text = "What is ransomware?",
                SubCategory = sub3,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Malware that encrypts files and demands payment", IsCorrect = true },
                    new AnswerOption { Text = "Free software", IsCorrect = false },
                    new AnswerOption { Text = "A security update", IsCorrect = false },
                    new AnswerOption { Text = "A backup tool", IsCorrect = false }
                }
            };

            var q15 = new Question
            {
                Text = "What is a trojan horse?",
                SubCategory = sub3,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Malware disguised as legitimate software", IsCorrect = true },
                    new AnswerOption { Text = "A strong password", IsCorrect = false },
                    new AnswerOption { Text = "An antivirus program", IsCorrect = false }
                }
            };

            var q16 = new Question
            {
                Text = "What is a firewall?",
                SubCategory = sub4,
                AnswerOptions =
                {
                    new AnswerOption { Text = "A wall made of fire", IsCorrect = false },
                    new AnswerOption { Text = "Network security system that monitors traffic", IsCorrect = true },
                    new AnswerOption { Text = "A type of virus", IsCorrect = false }
                }
            };

            var q17 = new Question
            {
                Text = "What does HTTPS mean?",
                SubCategory = sub4,
                AnswerOptions =
                {
                    new AnswerOption { Text = "HyperText Transfer Protocol Secure", IsCorrect = true },
                    new AnswerOption { Text = "High Tech Password System", IsCorrect = false },
                    new AnswerOption { Text = "Home Transfer Protocol Service", IsCorrect = false }
                }
            };

            var q18 = new Question
            {
                Text = "What is a VPN used for?",
                SubCategory = sub4,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Playing games faster", IsCorrect = false },
                    new AnswerOption { Text = "Encrypting internet connection and hiding IP", IsCorrect = true },
                    new AnswerOption { Text = "Downloading files", IsCorrect = false },
                    new AnswerOption { Text = "Sending emails", IsCorrect = false }
                }
            };

            var q19 = new Question
            {
                Text = "Why should you avoid public Wi-Fi for sensitive transactions?",
                SubCategory = sub4,
                AnswerOptions =
                {
                    new AnswerOption { Text = "It's too slow", IsCorrect = false },
                    new AnswerOption { Text = "It's insecure and can be intercepted", IsCorrect = true },
                    new AnswerOption { Text = "It's too expensive", IsCorrect = false }
                }
            };

            var q20 = new Question
            {
                Text = "What is encryption used for in network security?",
                SubCategory = sub4,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Protecting data in transit", IsCorrect = true },
                    new AnswerOption { Text = "Making data larger", IsCorrect = false },
                    new AnswerOption { Text = "Deleting data", IsCorrect = false },
                    new AnswerOption { Text = "Slowing down networks", IsCorrect = false }
                }
            };

            var q21 = new Question
            {
                Text = "What is social engineering?",
                SubCategory = sub5,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Building social networks", IsCorrect = false },
                    new AnswerOption { Text = "Manipulating people to divulge confidential info", IsCorrect = true },
                    new AnswerOption { Text = "Engineering social media apps", IsCorrect = false }
                }
            };

            var q22 = new Question
            {
                Text = "What is pretexting?",
                SubCategory = sub5,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Sending text messages", IsCorrect = false },
                    new AnswerOption { Text = "Creating fabricated scenario to steal info", IsCorrect = true },
                    new AnswerOption { Text = "Writing code", IsCorrect = false },
                    new AnswerOption { Text = "Testing software", IsCorrect = false }
                }
            };

            var q23 = new Question
            {
                Text = "What should you do before clicking a link?",
                SubCategory = sub5,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Click immediately", IsCorrect = false },
                    new AnswerOption { Text = "Verify the source and hover to check URL", IsCorrect = true },
                    new AnswerOption { Text = "Share with friends first", IsCorrect = false }
                }
            };

            var q24 = new Question
            {
                Text = "What is baiting in social engineering?",
                SubCategory = sub5,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Offering something enticing to trap victims", IsCorrect = true },
                    new AnswerOption { Text = "Fishing technique", IsCorrect = false },
                    new AnswerOption { Text = "A type of firewall", IsCorrect = false },
                    new AnswerOption { Text = "Email spam filter", IsCorrect = false }
                }
            };

            var q25 = new Question
            {
                Text = "How can you protect yourself from social engineering?",
                SubCategory = sub5,
                AnswerOptions =
                {
                    new AnswerOption { Text = "Trust everyone", IsCorrect = false },
                    new AnswerOption { Text = "Be skeptical and verify requests", IsCorrect = true },
                    new AnswerOption { Text = "Share all information freely", IsCorrect = false }
                }
            };

            db.Categories.Add(cat);
            await db.SaveChangesAsync();
        }
    }
}