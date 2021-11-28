using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Constants;
using QuizApp.Data;
using QuizApp.Models.APIRequests;
using QuizApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly int _resultPerPage = 10;

        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null)
                return BadRequest();

            var user = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.UserRole);
                return Ok();
            }
                
            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = Constants.Roles.AdminRole)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = Constants.Roles.AdminRole)]
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

        [HttpPost]
        [Authorize(Roles = Constants.Roles.AdminRole)]
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

        [HttpGet]
        [Authorize(Roles = Constants.Roles.AdminRole)]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = Constants.Roles.AdminRole)]
        public async Task<IActionResult> EditUsername(string id, string username)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(username))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            user.UserName = username;

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Edit), new { id = id });

        }

        [HttpPost]
        [Authorize(Roles = Constants.Roles.AdminRole)]
        public async Task<IActionResult> EditEmail(string id, string email)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(email))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            user.Email = email;

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Edit), new { id = id });
        }

        [HttpPost]
        [Authorize(Roles = Constants.Roles.AdminRole)]
        public async Task<IActionResult> EditPassword(string id, string password)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))
                return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, password);

            return RedirectToAction(nameof(Edit), new { id = id });
        }
    }
}
