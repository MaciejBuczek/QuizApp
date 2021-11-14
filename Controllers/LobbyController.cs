using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Data.Implementations;
using QuizApp.Data.Interfaces;
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
        public IActionResult Create(int quizId)
        {
            var lobby = new Lobby()
            {
                OwnerUsername = User.Identity.Name,
            };
            var quiz = _db.Quizzes.Where(q => q.Id == quizId).Include(q => q.Questions).ThenInclude(q => q.Answers).Include(q => q.CreatedBy).FirstOrDefault();

            if (quiz == null)
                return NotFound();
            
            var quizRunner = new QuizRunner(quiz);
            var code = _quizManager.GetQuizCode();

            _quizManager.AddQuiz(lobby, quizRunner, code);

            var lobbyVM = new LobbyVM()
            {
                Quiz = quiz,
                LobbyCode = code,
                IsOwner = true
            };
            return View(nameof(Index), lobbyVM);
        }

        public IActionResult Quiz(string lobbyCode)
        {
            if (string.IsNullOrEmpty(lobbyCode))
                return NotFound();

            return View(nameof(Quiz), lobbyCode);
        }

        public IActionResult Summary(string lobbyCode)
        {
            if (string.IsNullOrEmpty(lobbyCode))
                return NotFound();

            var quizRunner = _quizManager.GetQuizRunner(lobbyCode);

            var vm = new SummaryVM
            {
                LobbyCode = lobbyCode,
                UserScores = quizRunner.UserScores.OrderBy(us => us.Score).ThenBy(us => us.Username).ToList()
            };

            return View(vm);
        }
    }
}
