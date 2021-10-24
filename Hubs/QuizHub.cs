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

        public QuizHub(ILobbyManager lobbyManager)
        {
            _lobbyManager = lobbyManager;
        }

        public async Task ConnectToQuiz(string lobbyCode)
        {
            var lobby = _lobbyManager.GetLobby(lobbyCode);
            var quizInfo = new
            {
                QuizTitle = lobby.QuizRunner.Quiz.Title,
                Users = lobby.QuizRunner.UserScores.Select(u => u.Username)
            };
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
            await Clients.Caller.SendAsync("initalizeQuiz", quizInfo);
        }
    }
}
