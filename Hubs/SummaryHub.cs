using Microsoft.AspNetCore.SignalR;
using QuizApp.Constants.HubEnumerables;
using QuizApp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Hubs
{
    public class SummaryHub : Hub
    {
        private readonly IQuizManager _quizManager;

        public SummaryHub(IQuizManager quizManager)
        {
            _quizManager = quizManager;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if(Context.Items.TryGetValue(SummaryContextItems.LobbyCode, out var lobbyCode))
            {
                _quizManager.RemoveQuiz((string)lobbyCode);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public void ConnectToSummary(string lobbyCode)
        {
            Context.Items.Add(SummaryContextItems.LobbyCode, lobbyCode);
        }
    }
}
