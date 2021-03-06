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

        [Authorize]
        public IActionResult Create(int quizId)
        {
            var lobby = new Lobby()
            {
                OwnerUsername = _db.Quizzes.Where(q => q.Id == quizId).Include(q => q.CreatedBy).
                    Select(q => q.CreatedBy.UserName).FirstOrDefault(),
                Private = true,
                Code = _lobbyManager.GetLobbyCode()
            };
            _lobbyManager.AddLobby(lobby);
            var lobbyVM = new LobbyVM()
            {
                Quiz = _db.Quizzes.Where(q => q.Id == quizId).Include(q => q.Questions).Include(q => q.CreatedBy).FirstOrDefault(),
                LobbyCode = lobby.Code,
                IsOwner = true
            };
            return View(nameof(Index), lobbyVM);
        }
    }
}
