using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Data.Implementations;
using QuizApp.Data.Interfaces;
using QuizApp.Models.ViewModels;
using QuizApp.Views.Lobby;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Controllers
{
    public class LobbyController : Controller
    {
        private readonly ILobbyManager _lobbyManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;

        public LobbyController(ILobbyManager lobbyManager, UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _lobbyManager = lobbyManager;
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
            if(_lobbyManager.GetLobby(lobbyCode) == null)
            {
                return null;
            }
            return Json(new { redirectUrl = Url.Action("Join", "Lobby", new { lobbyCode = lobbyCode }) });
        }

        public IActionResult Join(string lobbyCode)
        {
            var lobby = _lobbyManager.GetLobby(lobbyCode);
            var lobbyVM = new LobbyVM()
            {
                Quiz = lobby.Quiz,
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
                LobbyStatus = LobbyStatus.Open,
                Quiz = _db.Quizzes.Where(q => q.Id == quizId).Include(q => q.CreatedBy)
                    .Include(q => q.Questions).ThenInclude(q => q.Answers).FirstOrDefault(),
                Private = true,
                Code = _lobbyManager.GetLobbyCode()
            };
            _lobbyManager.AddLobby(lobby);
            var lobbyVM = new LobbyVM()
            {
                Quiz = lobby.Quiz,
                LobbyCode = lobby.Code,
                IsOwner = true
            };
            return View(nameof(Index), lobbyVM);
        }

        public IActionResult Quiz(string lobbyCode)
        {
            return View(nameof(Quiz),lobbyCode);
        }
    }
}
