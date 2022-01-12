using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using QuizApp.Constants.HubEnumerables;
using QuizApp.Data.Interfaces;
using System;
using System.Threading.Tasks;

namespace QuizApp.Hubs
{
    public class LobbyHub : Hub
    {
        private readonly IQuizManager _quizManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LobbyHub(IQuizManager quizManager, UserManager<IdentityUser> userManager)
        {
            _quizManager = quizManager;
            _userManager = userManager;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if(Context.Items.TryGetValue(QuizContextItems.LobbyCode, out var lobbyCode) &&
                Context.Items.TryGetValue(QuizContextItems.Removable, out var removable))
            {
                if (!(bool)removable)
                    return;

                var lobby = _quizManager.GetLobby((string)lobbyCode);
                if (lobby != null)
                {
                    var username = GetUsername();
                    if (username == null)
                        return;

                    if (lobby.OwnerUsername == username)
                    {
                        _quizManager.RemoveQuiz((string)lobbyCode);
                        await Clients.GroupExcept((string)lobbyCode, Context.ConnectionId).SendAsync("closeLobby");
                    }
                    else
                    {
                        lobby.ConnectedUsers.Remove(username);
                        await Clients.GroupExcept((string)lobbyCode, Context.ConnectionId).SendAsync("initializeUsers", lobby);
                    }
                }
            }
        }

        public async Task ConnectOwnerToLobby(string lobbyCode)
        {
            Context.Items.Add(QuizContextItems.LobbyCode, lobbyCode);
            Context.Items.Add(QuizContextItems.Removable, true);

            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
            await Clients.Caller.SendAsync("initializeUsers", _quizManager.GetLobby(lobbyCode));
        }

        public async Task ConnectToLobby(string lobbyCode)
        {
            Context.Items.Add(QuizContextItems.LobbyCode, lobbyCode);
            Context.Items.Add(QuizContextItems.Removable, true);

            var username = GetUsername();
            if (username == null)
                return;

            _quizManager.GetLobby(lobbyCode).ConnectedUsers.Add(username);

            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
            await Groups.AddToGroupAsync(Context.ConnectionId, username);

            await Clients.GroupExcept(lobbyCode, Context.ConnectionId).SendAsync("addUser", username);
            await Clients.Caller.SendAsync("initializeUsers", _quizManager.GetLobby(lobbyCode));
        }

        public async Task ConnectToLobbyMobile(string lobbyCode, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return;

            Context.Items.Add(QuizContextItems.Username, user.UserName);
            await ConnectToLobby(lobbyCode);
        }

        public async Task BeginQuiz(string lobbyCode)
        {
            var lobby = _quizManager.GetLobby(lobbyCode);
            lobby.UsersConnectedAtStart = lobby.ConnectedUsers.Count + 1;

            Context.Items[QuizContextItems.Removable] = false;

            await Clients.Group(lobbyCode).SendAsync("redirectToQuiz", ("/Lobby/Quiz?lobbyCode=" + lobbyCode));
        }

        public async Task KickUser(string username)
        {
            await Clients.Groups(username).SendAsync("kick");
        }

        private string GetUsername()
        {
            var username = Context.User.Identity.Name;
            if (username == null)
            {
                if (Context.Items.TryGetValue(QuizContextItems.Username, out var value))
                {
                    username = (string)value;
                }
                else
                    return null;
            }
            return username;
        }
    }
}
