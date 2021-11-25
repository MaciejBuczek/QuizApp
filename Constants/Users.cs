using Microsoft.AspNetCore.Identity;

namespace QuizApp.Constants
{
    public static class Users
    {
        public static (IdentityUser User, string Password) Admin { get; } = (new IdentityUser
        {
            UserName = "admin",
            Email = "admin@email.com"
        },
            "Admin1!");
    }
}
