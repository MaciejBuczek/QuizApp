using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models.ViewModels
{
    public class UserVM : PaginationVM
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public List<IdentityUser> Users { get; set; }
    }
}
