using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Models;
using QuizApp.Models.ViewModels;
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
        public IActionResult Search(string search)
        {
            var quizList = _db.Quizzes.Where(q => q.Title.Contains(search) || search == null).Include(q => q.Questions).ToList();
            return View(nameof(Index), quizList);
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
            if (_db.Quizzes.Find(id) == null)
                return NotFound();
            var quizVM = _db.Quizzes.Where(q => q.Id == id).Select(q => new QuizVM
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description,
                NegativePoints = q.NegativePoints,
                PartialPoints = q.PartialPoints,
                Questions = _db.Questions.Where(qu => qu.IdQuiz == q.Id).Select(qu => new QuestionVM
                {
                    Content = qu.Content,
                    Time = qu.Time,
                    Points = qu.Points,
                    CorrectAnswer = qu.CorrectAnswer,
                    Answers = _db.Answers.Where(a => a.IdQuestion == qu.Id).Select(a => new AnswerVM
                    {
                        Content = a.Content
                    }).ToList()
                }).ToList()
            }).First();
            quizVM.Questions.ForEach(q => q.SetAnswers());
            return View(quizVM);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(Quiz quiz)
        {
            quiz.UserId = _userManager.GetUserId(User);
            quiz.CreatedAt = DateTime.Now;
            foreach (var question in quiz.Questions)
            {
                question.Quiz = quiz;
                foreach (var answer in question.Answers)
                {
                    answer.Question = question;
                }
            }
            _db.Quizzes.Remove(_db.Quizzes.Find(quiz.Id));
            _db.SaveChanges();
            quiz.Id = 0;
            _db.Quizzes.Add(quiz);
            _db.SaveChanges();
            return Json(new { redirectUrl = Url.Action("MyQuizzes", "Quiz") });
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
