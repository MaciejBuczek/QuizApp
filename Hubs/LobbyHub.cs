using Microsoft.AspNetCore.SignalR;
using QuizApp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Hubs
{
    public class LobbyHub : Hub
    {
        private readonly ILobbyManager _lobbyManager;

        public LobbyHub(ILobbyManager lobbyManager)
        {
            _lobbyManager = lobbyManager;
        }

        public async Task ConnectOwnerToLobby(string lobbyCode)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
            await Clients.Caller.SendAsync("initializeUsers", _lobbyManager.GetLobby(lobbyCode));
        }

        public async Task ConnectToLobby(string lobbyCode)
        {
            _lobbyManager.GetLobby(lobbyCode).ConnectedUsers.Add(Context.User.Identity.Name);
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
            await Clients.GroupExcept(lobbyCode, Context.ConnectionId).SendAsync("addUser", Context.User.Identity.Name);
            await Clients.Caller.SendAsync("initializeUsers", _lobbyManager.GetLobby(lobbyCode));
        }
    }
}
