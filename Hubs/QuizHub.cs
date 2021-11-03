using Microsoft.AspNetCore.SignalR;
using QuizApp.Data.Implementations;
using QuizApp.Data.Interfaces;
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Hubs
{

    public class QuizHub : Hub
    {
        private readonly IQuizManager _quizManager;

        public QuizHub(IQuizManager quizManager)
        {
            _quizManager = quizManager;
        }

        public async Task ConnectToQuiz(string lobbyCode)
        {
            var quizRunner = _quizManager.GetQuizRunner(lobbyCode);
            var quizLobby = _quizManager.GetLobby(lobbyCode);
            quizRunner.UserScores.Add( new UserScore { Username = Context.User.Identity.Name, Score = 0 });
            var quizInfo = new
            {
                QuizTitle = _quizManager.GetQuiz(lobbyCode).Title,
                UsersScores = quizRunner.UserScores
            };
            quizInfo.UsersScores.OrderByDescending(us => us.Username);
            await Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
            await Clients.Caller.SendAsync("initalizeQuiz", quizInfo);
            await Clients.GroupExcept(lobbyCode, Context.ConnectionId).SendAsync("updateScoreboard", quizInfo.UsersScores);

            if(quizLobby.UsersConnectedAtStart == quizRunner.UserScores.Count)
            {
               BeginQuiz(lobbyCode);
            }
        }

        private void BeginQuiz(string lobbyCode)
        {
            var quizRunner = _quizManager.GetQuizRunner(lobbyCode);
            var quiz = _quizManager.GetQuiz(lobbyCode);

            quizRunner.PrepareQuizForUsers(quiz);
        }     

    }
}