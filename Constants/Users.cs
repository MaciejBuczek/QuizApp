using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
