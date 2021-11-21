using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Data.Implementations;
using QuizApp.Data.Interfaces;
using QuizApp.Models;
using QuizApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Controllers
{
    [Authorize]
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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckCode(string lobbyCode)
        {
            if(_quizManager.GetLobby(lobbyCode) == null)
            {
                return null;
            }
            return Json(new { redirectUrl = Url.Action("Join", "Lobby", new { lobbyCode = lobbyCode }) });
        }

        [HttpGet]
        public IActionResult RegenerateCode(string previousCode)
        {
            if (string.IsNullOrEmpty(previousCode) || _quizManager.GetLobby(previousCode) == null)
                return NotFound();

            var newCode = _quizManager.RegenerateCode(previousCode);

            return Json(new { newCode = newCode });
        }

        [HttpGet]
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

        [Authorize]
        [HttpGet]
        public IActionResult Create(int quizId)
        {
            if (_db.Quizzes.Find(quizId) == null)
                return NotFound();

            var vm = GenerateQuizVM(quizId);

            return View(nameof(Index), vm);
        }

        [HttpGet]
        public IActionResult Quiz(string lobbyCode)
        {
            if (string.IsNullOrEmpty(lobbyCode))
                return NotFound();

            return View(nameof(Quiz), lobbyCode);
        }

        [HttpGet]
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
    }
}
