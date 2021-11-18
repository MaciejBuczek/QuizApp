using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Data;
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Controllers
{
    [Authorize]
    public class RatingController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;

        public RatingController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(int rating, int quizId)
        {
            var userId = _userManager.GetUserId(User);

            var userPreviousRating = _db.Ratings.Where(r => r.UserId == userId && r.IdQuiz == quizId).FirstOrDefault();

            if(userPreviousRating != null)
            {
                _db.Ratings.Remove(userPreviousRating);
                _db.SaveChanges();
            }

            _db.Ratings.Add(new Rating
            {
                Content = rating,
                UserId = userId,
                IdQuiz = quizId
            });
            _db.SaveChanges();

            return View();
        }
    }
}
