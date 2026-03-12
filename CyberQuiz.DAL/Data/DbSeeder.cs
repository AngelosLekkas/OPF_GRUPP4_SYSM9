using CyberQuiz.DAL.Entities;
//using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CyberQuiz.DAL.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(
        CyberQuizDbContext db)
        //UserManager<AppUser> userManager,
        //RoleManager<IdentityRole> roleManager)
    {
        ArgumentNullException.ThrowIfNull(db);
        //ArgumentNullException.ThrowIfNull(userManager);
        //ArgumentNullException.ThrowIfNull(roleManager);

		// Ensure the host applies migrations before calling the seeder.
		// (Avoid doing db.Database.MigrateAsync() here to keep seeding and schema management separate.)

		// 1. Seed Identity Roles (User, Admin)
        //await SeedRolesAsync(roleManager);

        // 3. Seed Default Users (user, admin) using transaction logic if needed
        //await SeedUsersAsync(userManager);

		// 3. Seed Quiz Content (Categories, SubCategories, Questions, Answers)
        await SeedQuizDataAsync(db);
    }

    //private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    //{
    //    string[] roles = { "User", "Admin" };

    //    foreach (var role in roles)
    //    {
    //        if (!await roleManager.RoleExistsAsync(role))
    //        {
    //            var result = await roleManager.CreateAsync(new IdentityRole(role));
    //            EnsureSucceeded(result, $"Failed to create role '{role}'.");
    //        }
    //    }
    //}



    //========================
    // SEED USER & ADMIN
    //========================
    //private static async Task SeedUsersAsync(UserManager<AppUser> userManager)
    //{
    //    // Seed regular user
    //    if (await userManager.FindByNameAsync("user") == null)
    //    {
    //        var user = new AppUser
    //        {
    //            UserName = "user",
    //            Email = "user@example.com",
    //            EmailConfirmed = true,
    //            FullName = "Regular User"
    //        };
    //        var result = await userManager.CreateAsync(user, "Password1234!");
    //        if (result.Succeeded)
    //        {
    //            await userManager.AddToRoleAsync(user, "User");
    //        }
    //    }

    //    // Seed admin user
    //    if (await userManager.FindByNameAsync("admin") == null)
    //    {
    //        var admin = new AppUser
    //        {
    //            UserName = "admin",
    //            Email = "admin@cyberquiz.com",
    //            EmailConfirmed = true,
    //            FullName = "System Administrator"
    //        };
    //        var result = await userManager.CreateAsync(admin, "Admin1234!");
    //        if (result.Succeeded)
    //        {
    //            await userManager.AddToRoleAsync(admin, "Admin");
    //        }
    //    }
    //}





    //======================================================================
    // SEED QUIZ DATA (Categories, SubCategories, Questions, AnswerOptions)
    //======================================================================
    private static async Task SeedQuizDataAsync(CyberQuizDbContext db)
    {
        // Check if data already exists to avoid duplication
        if (await db.Categories.AnyAsync())
        {
            return; // Data already seeded
        }

        // Use transaction for data integrity (Atomicity)
        await using var transaction = await db.Database.BeginTransactionAsync();
        try
        {
            // ====================================================================================
            // CATEGORY 1: Network Security
            // ====================================================================================
            var cat1 = new Category { Name = "Network Security" };

            var sub1_1 = new SubCategory { Name = "Network Attacks", Category = cat1, SortOrder = 1 };
            sub1_1.Questions = new List<Question>
{
    new Question {
        Text = "What is a DDoS attack?",
        SubCategory = sub1_1,
        AnswerOptions = {
            new AnswerOption { Text = "Overloading a server with massive traffic", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Encrypting stored passwords", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Updating firewall firmware", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "What is phishing?",
        SubCategory = sub1_1,
        AnswerOptions = {
            new AnswerOption { Text = "Tricking users into revealing sensitive information", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Scanning open ports", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Monitoring CPU usage", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "What is a Man-in-the-Middle attack?",
        SubCategory = sub1_1,
        AnswerOptions = {
            new AnswerOption { Text = "Intercepting communication between two parties", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Deleting system logs", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Restarting network routers", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "Which tool is used for packet sniffing?",
        SubCategory = sub1_1,
        AnswerOptions = {
            new AnswerOption { Text = "Wireshark", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Excel", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Docker", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "What is port scanning?",
        SubCategory = sub1_1,
        AnswerOptions = {
            new AnswerOption { Text = "Identifying open network ports", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Encrypting hard drives", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Installing software updates", IsCorrect = false, DisplayOrder = 3 }
        }
    }
};

            var sub1_2 = new SubCategory { Name = "Secure Communication", Category = cat1, SortOrder = 2 };
            sub1_2.Questions = new List<Question>
{
    new Question {
        Text = "What does HTTPS ensure?",
        SubCategory = sub1_2,
        AnswerOptions = {
            new AnswerOption { Text = "Encrypted web traffic", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Faster internet speed", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Unlimited bandwidth", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "Which protocol is secure for file transfers?",
        SubCategory = sub1_2,
        AnswerOptions = {
            new AnswerOption { Text = "SFTP", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "FTP", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Telnet", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "What is TLS mainly used for?",
        SubCategory = sub1_2,
        AnswerOptions = {
            new AnswerOption { Text = "Encrypting data in transit", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Managing databases", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Compressing files", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "What is a VPN?",
        SubCategory = sub1_2,
        AnswerOptions = {
            new AnswerOption { Text = "Encrypted tunnel over the internet", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Local printer network", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Gaming protocol", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "Which port is used for HTTPS?",
        SubCategory = sub1_2,
        AnswerOptions = {
            new AnswerOption { Text = "443", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "80", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "25", IsCorrect = false, DisplayOrder = 3 }
        }
    }
};

            // ====================================================================================
            // CATEGORY 2: Cyber Defense
            // ====================================================================================
            var cat2 = new Category { Name = "Cyber Defense" };

            var sub2_1 = new SubCategory { Name = "Authentication Security", Category = cat2, SortOrder = 1 };
            sub2_1.Questions = new List<Question>
{
    new Question {
        Text = "What is Multi-Factor Authentication (MFA)?",
        SubCategory = sub2_1,
        AnswerOptions = {
            new AnswerOption { Text = "Using multiple verification methods", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Logging in twice", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Sharing passwords", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "Why are strong passwords important?",
        SubCategory = sub2_1,
        AnswerOptions = {
            new AnswerOption { Text = "They reduce risk of brute-force attacks", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "They speed up login", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "They reduce internet cost", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "What is password hashing?",
        SubCategory = sub2_1,
        AnswerOptions = {
            new AnswerOption { Text = "Transforming passwords into fixed encrypted values", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Saving passwords as plain text", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Deleting passwords automatically", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "What is biometric authentication?",
        SubCategory = sub2_1,
        AnswerOptions = {
            new AnswerOption { Text = "Authentication using physical traits", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Using security questions", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Using IP addresses", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "What is a brute-force attack?",
        SubCategory = sub2_1,
        AnswerOptions = {
            new AnswerOption { Text = "Trying many password combinations automatically", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Updating antivirus", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Scanning USB devices", IsCorrect = false, DisplayOrder = 3 }
        }
    }
};

            var sub2_2 = new SubCategory { Name = "Malware Protection", Category = cat2, SortOrder = 2 };
            sub2_2.Questions = new List<Question>
{
    new Question {
        Text = "What is ransomware?",
        SubCategory = sub2_2,
        AnswerOptions = {
            new AnswerOption { Text = "Malware that locks data for payment", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Backup software", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Firewall update", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "What does antivirus software do?",
        SubCategory = sub2_2,
        AnswerOptions = {
            new AnswerOption { Text = "Detects and removes malicious software", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Speeds up internet", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Encrypts emails", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "What is spyware?",
        SubCategory = sub2_2,
        AnswerOptions = {
            new AnswerOption { Text = "Software that secretly gathers user data", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Gaming software", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Printer drivers", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "What is a Trojan?",
        SubCategory = sub2_2,
        AnswerOptions = {
            new AnswerOption { Text = "Malware disguised as legitimate software", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Cloud storage", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Security patch", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "Why are software updates important?",
        SubCategory = sub2_2,
        AnswerOptions = {
            new AnswerOption { Text = "They fix security vulnerabilities", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "They reduce RAM usage", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "They remove encryption", IsCorrect = false, DisplayOrder = 3 }
        }
    }
};

            // ====================================================================================
            // CATEGORY 3: Security Architecture
            // ====================================================================================
            var cat3 = new Category { Name = "Security Architecture" };

            var sub3_1 = new SubCategory { Name = "Secure System Design", Category = cat3, SortOrder = 1 };
            sub3_1.Questions = new List<Question>
{
    new Question {
        Text = "What is the principle of least privilege?",
        SubCategory = sub3_1,
        AnswerOptions = {
            new AnswerOption { Text = "Users get only necessary access rights", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Admins get full control always", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Everyone shares accounts", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "What is network segmentation?",
        SubCategory = sub3_1,
        AnswerOptions = {
            new AnswerOption { Text = "Dividing a network into smaller secure parts", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Deleting routers", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Increasing bandwidth", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "What is an IDS?",
        SubCategory = sub3_1,
        AnswerOptions = {
            new AnswerOption { Text = "Intrusion Detection System", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Internet Download Service", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Internal Data Storage", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "Why is logging important in security?",
        SubCategory = sub3_1,
        AnswerOptions = {
            new AnswerOption { Text = "To detect and investigate incidents", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "To increase CPU usage", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "To slow down systems", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "What is a security audit?",
        SubCategory = sub3_1,
        AnswerOptions = {
            new AnswerOption { Text = "Reviewing systems for vulnerabilities", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Deleting databases", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Installing printers", IsCorrect = false, DisplayOrder = 3 }
        }
    }
};

            var sub3_2 = new SubCategory { Name = "Database Security", Category = cat3, SortOrder = 2 };
            sub3_2.Questions = new List<Question>
{
    new Question {
        Text = "What is SQL Injection?",
        SubCategory = sub3_2,
        AnswerOptions = {
            new AnswerOption { Text = "Injecting malicious SQL queries into input fields", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Backing up databases", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Encrypting network cables", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "Why encrypt data at rest?",
        SubCategory = sub3_2,
        AnswerOptions = {
            new AnswerOption { Text = "To protect stored sensitive information", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "To increase internet speed", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "To remove backups", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "What is database access control?",
        SubCategory = sub3_2,
        AnswerOptions = {
            new AnswerOption { Text = "Restricting who can read or modify data", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Deleting user accounts", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Formatting hard drives", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "What is data masking?",
        SubCategory = sub3_2,
        AnswerOptions = {
            new AnswerOption { Text = "Hiding sensitive data from unauthorized users", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "Deleting rows", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "Restarting servers", IsCorrect = false, DisplayOrder = 3 }
        }
    },
    new Question {
        Text = "Why are database backups important?",
        SubCategory = sub3_2,
        AnswerOptions = {
            new AnswerOption { Text = "To recover data after attacks or failures", IsCorrect = true, DisplayOrder = 1 },
            new AnswerOption { Text = "To increase hacking risk", IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Text = "To slow performance", IsCorrect = false, DisplayOrder = 3 }
        }
    }
    };





            //============================================================================================
            // TRANSITION: Save Categories with SubCategories, Questions, and Answers(save all or nothing)
            //============================================================================================
            await db.Categories.AddRangeAsync(cat1, cat2, cat3);

            await db.SubCategories.AddRangeAsync(
            sub1_1, sub1_2,
            sub2_1, sub2_2,
            sub3_1, sub3_2
            );

            await db.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    //private static void EnsureSucceeded(IdentityResult result, string message)
    //{
    //    if (result.Succeeded)
    //    {
    //        return;
    //    }

    //    var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
    //    throw new InvalidOperationException($"{message} Errors: {errors}");
    //}
}