using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Data;
using QuizApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Controllers
{
    [Authorize(Roles = Constants.Roles.AdminRole)]
    public class UserController : Controller
    {
        private ApplicationDbContext _db;
        private UserManager<IdentityUser> _userManager;
        private readonly int _resultPerPage = 10;

        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search(string username, string email, int? targetPage)
        {
            var userQuery = _db.Users
                .Where(u => ((username == null || u.UserName.Contains(username)) && (email == null || u.Email.Contains(email))));

            var userVM = new UserVM
            {
                CurrentPage = targetPage?? 1,
                TotalPages = (userQuery.Count() + _resultPerPage - 1) / _resultPerPage,
                Username = username,
                Email = email,
                Users = userQuery.Skip(_resultPerPage * (targetPage - 1) ?? 0).Take(_resultPerPage).ToList()
            };

            return View(nameof(Index), userVM);
        }

        public async Task<IActionResult> Remove(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Search));
        }
    }
}
