using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Controllers
{
    public class QuizController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public QuizController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyQuizzes()
        {
            var quizzList = _db.Quizzes.Where(q => q.UserId == _userManager.GetUserId(User)).Include(q => q.Questions).ToList();
            return View(quizzList);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Quiz quiz)
        {
            quiz.UserId = _userManager.GetUserId(User);
            quiz.CreatedAt = DateTime.Now;
            foreach(var question in quiz.Questions)
            {
                question.Quiz = quiz;
                foreach(var answer in question.Answers)
                {
                    answer.Question = question;
                }
            }
            _db.Add(quiz);
            _db.SaveChanges();
            return Json(new { redirectUrl = Url.Action("MyQuizzes", "Quiz") });
        }

        public IActionResult Edit(int id)
        {
            var quiz = _db.Quizzes.Find(id);
            if (quiz == null)
                return NotFound();
            return View(quiz);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Remove(int id)
        {
            var quiz = _db.Quizzes.Find(id);
            if (quiz == null)
                return NotFound();
            _db.Remove(quiz);
            _db.SaveChanges();
            return Json(new { redirectUrl = Url.Action("MyQuizzes", "Quiz") });
        }
    }
}
