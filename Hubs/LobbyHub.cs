using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using QuizApp.Data.Interfaces;
using QuizApp.Views.Lobby;
using System.Threading.Tasks;

namespace QuizApp.Hubs
{
    public class LobbyHub : Hub
    {
        private readonly ILobbyManager _lobbyManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LobbyHub(ILobbyManager lobbyManager, UserManager<IdentityUser> userManager)
        {
            _lobbyManager = lobbyManager;
            _userManager = userManager;
        }

        public async Task ConnectOwnerToLobby(string lobbyCode)
        {
            var lobby = _lobbyManager.GetLobby(lobbyCode);
            if (lobby.LobbyStatus == LobbyStatus.Open)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
                await Clients.Caller.SendAsync("initializeUsers", _lobbyManager.GetLobby(lobbyCode));
            }
        }

        public async Task ConnectToLobby(string lobbyCode)
        {
            var lobby = _lobbyManager.GetLobby(lobbyCode);
            if (lobby.LobbyStatus == LobbyStatus.Open)
            {
                lobby.ConnectedUsers.Add(Context.User.Identity.Name);
                await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
                await Clients.GroupExcept(lobbyCode, Context.ConnectionId).SendAsync("addUser", Context.User.Identity.Name);
                await Clients.Caller.SendAsync("initializeUsers", _lobbyManager.GetLobby(lobbyCode));
            }
        }

        public async Task BeginQuiz(string lobbyCode)
        {
            var lobby = _lobbyManager.GetLobby(lobbyCode);
            lobby.LobbyStatus = LobbyStatus.Closed;
            lobby.GenerateUserFlags();
            await Clients.Group(lobbyCode).SendAsync("redirectToQuiz", ("/Lobby/Quiz?lobbyCode=" + lobbyCode));
        }
    }
}
