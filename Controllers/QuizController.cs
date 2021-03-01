using Microsoft.AspNetCore.Mvc;
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

        public QuizController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Quiz quiz)
        {
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
            return View();
        }
    }
}
