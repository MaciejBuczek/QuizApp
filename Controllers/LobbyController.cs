using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Data.Implementations;
using QuizApp.Data.Interfaces;
using QuizApp.Models;
using QuizApp.Models.API;
using QuizApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuizApp.Controllers
{
    public class LobbyController : Controller
    {
        private readonly IQuizManager _quizManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;

        public LobbyController(IQuizManager quizManager, UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _quizManager = quizManager;
            _userManager = userManager;
            _db = db;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CheckCode(string lobbyCode)
        {
            if(_quizManager.GetLobby(lobbyCode) == null)
            {
                return NotFound();
            }
            return Json(new { redirectUrl = Url.Action("Join", "Lobby", new { lobbyCode = lobbyCode }) });
        }

        [HttpGet]
        [Authorize]
        public IActionResult RegenerateCode(string previousCode)
        {
            if (string.IsNullOrEmpty(previousCode) || _quizManager.GetLobby(previousCode) == null)
                return NotFound();

            var newCode = _quizManager.RegenerateCode(previousCode);

            return Json(new { newCode = newCode });
        }

        [HttpGet]
        [Authorize]
        public IActionResult Join(string lobbyCode)
        {
            if (string.IsNullOrEmpty(lobbyCode))
                return NotFound();

            var lobby = _quizManager.GetLobby(lobbyCode);

            var lobbyVM = new LobbyVM()
            {
                Quiz = _quizManager.GetQuizRunner(lobbyCode).Quiz,
                LobbyCode = lobbyCode,
                IsOwner = false
            };
            return View(nameof(Index), lobbyVM);
        }

        [HttpGet]
        public IActionResult JoinAPI(string lobbyCode)
        {
            if (string.IsNullOrEmpty(lobbyCode))
                return NotFound();

            var quiz = _quizManager.GetQuizRunner(lobbyCode).Quiz;
            var response = new JoinResponse
            {
                Title = GetStringWithoutHTMLTags(quiz.Title),
                Description = GetStringWithoutHTMLTags(quiz.Description ?? string.Empty),
                Rating = quiz.Ratings.Count == 0 ? 0 : quiz.Ratings.Average(r => r.Content),
                TotalQuestions = quiz.Questions.Count,
                TotalTime = quiz.Questions.Sum(q => q.Time),
                TotalPoints = quiz.Questions.Sum(q => q.Points),
                CreatedOn = quiz.CreatedAt,
                CreatedBy = quiz.CreatedBy.UserName
            };

            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create(int quizId)
        {
            if (_db.Quizzes.Find(quizId) == null)
                return NotFound();

            var vm = GenerateQuizVM(quizId);

            return View(nameof(Index), vm);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Quiz(string lobbyCode)
        {
            if (string.IsNullOrEmpty(lobbyCode))
                return NotFound();

            return View(nameof(Quiz), lobbyCode);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Summary(string lobbyCode)
        {
            if (string.IsNullOrEmpty(lobbyCode))
                return NotFound();

            var quizRunner = _quizManager.GetQuizRunner(lobbyCode);
            var userId = _userManager.GetUserId(User);
            
            var previousRating = _db.Ratings.Where(r => r.UserId == userId && r.IdQuiz == quizRunner.Quiz.Id).FirstOrDefault();

            int? previousRatingScore = null;
            if (previousRating != null)
                previousRatingScore = previousRating.Content;

            var vm = new SummaryVM
            {
                LobbyCode = lobbyCode,
                UserScores = quizRunner.UserScores.OrderByDescending(us => us.Score).ThenBy(us => us.Username).ToList(),
                QuizId = quizRunner.Quiz.Id,
                PreviousRating = previousRatingScore
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult SummaryAPI(string lobbyCode)
        {
            if (string.IsNullOrEmpty(lobbyCode))
                return NotFound();

            var quizRunner = _quizManager.GetQuizRunner(lobbyCode);
            if (quizRunner == null)
                return NotFound();

            return Ok(quizRunner.UserScores);
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult Start(int quizId)
        {
            if (_db.Quizzes.Find(quizId) == null)
                return NotFound();

            var vm = GenerateQuizVM(quizId);

            _quizManager.GetLobby(vm.LobbyCode).UsersConnectedAtStart = 1;

            return View("Quiz", vm.LobbyCode);
        }

        private LobbyVM GenerateQuizVM(int quizId)
        {
            var lobby = new Lobby()
            {
                OwnerUsername = User.Identity.Name,
            };
            var quiz = _db.Quizzes
                .Where(q => q.Id == quizId)
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .Include(q => q.CreatedBy)
                .Include(q => q.Ratings)
                .FirstOrDefault();

            if (quiz.Ratings == null)
                quiz.Ratings = new List<Rating>();

            var quizRunner = new QuizRunner(quiz);
            var code = _quizManager.GetQuizCode();

            _quizManager.AddQuiz(lobby, quizRunner, code);

            var lobbyVM = new LobbyVM()
            {
                Quiz = quiz,
                LobbyCode = code,
                IsOwner = true
            };

            return lobbyVM;
        }

        private string GetStringWithoutHTMLTags(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}
