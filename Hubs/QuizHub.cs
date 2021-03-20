using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using QuizApp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Hubs
{
    public class QuizHub : Hub
    {
        private readonly ILobbyManager _lobbyManager;
        private readonly UserManager<IdentityUser> _userManager;

        public QuizHub(ILobbyManager lobbyManager, UserManager<IdentityUser> userManager)
        {
            _lobbyManager = lobbyManager;
            _userManager = userManager;
        }

        public async Task ConnectToQuiz(string lobbyCode)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
            var lobby = _lobbyManager.GetLobby(lobbyCode);
            lobby.SetUserFlag(_userManager.GetUserId(Context.User), true);
            if (lobby.AreAllUserFlagsActivated())
            {
                //await Clients.Group(lobbyCode).SendAsync("loadQuestion");
            }
        }
    }
}
