using Microsoft.AspNetCore.SignalR;
using QuizApp.Constants.HubEnumerables;
using QuizApp.Data.Interfaces;
using QuizApp.Data.Responses;
using System;
using System.Threading.Tasks;

namespace QuizApp.Hubs
{
    public class LobbyHub : Hub
    {
        private readonly IQuizManager _quizManager;

        public LobbyHub(IQuizManager quizManager)
        {
            _quizManager = quizManager;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if(Context.Items.TryGetValue(LobbyContextItems.LobbyCode, out var lobbyCode))
            {
                var lobby = _quizManager.GetLobby((string)lobbyCode);
                if (lobby != null)
                {
                    if (lobby.OwnerUsername == Context.User.Identity.Name)
                    {
                        _quizManager.RemoveQuiz((string)lobbyCode);
                        await Clients.GroupExcept((string)lobbyCode, Context.ConnectionId).SendAsync("closeLobby");
                    }
                    else
                    {
                        lobby.ConnectedUsers.Remove(Context.User.Identity.Name);
                        await Clients.GroupExcept((string)lobbyCode, Context.ConnectionId).SendAsync("initializeUsers", lobby);
                    }
                }
            }
        }

        public async Task ConnectOwnerToLobby(string lobbyCode)
        {
            Context.Items.Add(LobbyContextItems.LobbyCode, lobbyCode);
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
            await Clients.Caller.SendAsync("initializeUsers", _quizManager.GetLobby(lobbyCode));
        }

        public async Task ConnectToLobby(string lobbyCode)
        {
            Context.Items.Add(LobbyContextItems.LobbyCode, lobbyCode);
            _quizManager.GetLobby(lobbyCode).ConnectedUsers.Add(Context.User.Identity.Name);
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
            await Clients.GroupExcept(lobbyCode, Context.ConnectionId).SendAsync("addUser", Context.User.Identity.Name);
            await Clients.Caller.SendAsync("initializeUsers", _quizManager.GetLobby(lobbyCode));
        }

        public async Task BeginQuiz(string lobbyCode)
        {
            await Clients.Group(lobbyCode).SendAsync("redirectToQuiz", ("/Lobby/Quiz?lobbyCode=" + lobbyCode));
        }
    }
}
