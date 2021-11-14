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

        public LobbyHub(IQuizManager quizManager)
        {
            _quizManager = quizManager;
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
            Context.Items.Add(QuizContextItems.LobbyCode, lobbyCode);
            Context.Items.Add(QuizContextItems.Removable, true);

            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
            await Clients.Caller.SendAsync("initializeUsers", _quizManager.GetLobby(lobbyCode));
        }

        public async Task ConnectToLobby(string lobbyCode)
        {
            Context.Items.Add(QuizContextItems.LobbyCode, lobbyCode);
            Context.Items.Add(QuizContextItems.Removable, true);

            _quizManager.GetLobby(lobbyCode).ConnectedUsers.Add(Context.User.Identity.Name);

            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
            await Clients.GroupExcept(lobbyCode, Context.ConnectionId).SendAsync("addUser", Context.User.Identity.Name);
            await Clients.Caller.SendAsync("initializeUsers", _quizManager.GetLobby(lobbyCode));
        }

        public async Task BeginQuiz(string lobbyCode)
        {
            var lobby = _quizManager.GetLobby(lobbyCode);
            lobby.UsersConnectedAtStart = lobby.ConnectedUsers.Count + 1;

            Context.Items[QuizContextItems.Removable] = false;

            await Clients.Group(lobbyCode).SendAsync("redirectToQuiz", ("/Lobby/Quiz?lobbyCode=" + lobbyCode));
        }
    }
}
