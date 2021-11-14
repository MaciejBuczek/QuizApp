﻿using Microsoft.AspNetCore.SignalR;
using QuizApp.Constants.HubEnumerables;
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

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.Items.TryGetValue(QuizContextItems.LobbyCode, out var lobbyCode))
            {
                var quizRunner = _quizManager.GetQuizRunner((string)lobbyCode);
                if (!quizRunner.IsFinished)
                {
                    quizRunner.UserScores.Remove(quizRunner.UserScores.Where(us => us.Username == Context.User.Identity.Name).FirstOrDefault());
                    quizRunner.UserScores = quizRunner.UserScores.OrderByDescending(us => us.Score).ThenBy(us => us.Username).ToList();

                    Clients.Group((string)lobbyCode).SendAsync("updateScoreboard", quizRunner.UserScores);

                    if (quizRunner.UserScores.Count == 0)
                    {
                        _quizManager.RemoveQuiz((string)lobbyCode);
                    }
                }
            }

            return base.OnDisconnectedAsync(exception);
        }

        public Task ConnectToQuiz(string lobbyCode)
        {
            Context.Items.Add(QuizContextItems.LobbyCode, lobbyCode);

            var quizRunner = _quizManager.GetQuizRunner(lobbyCode);
            var quizLobby = _quizManager.GetLobby(lobbyCode);
            quizRunner.UserScores.Add( new UserScore { Username = Context.User.Identity.Name, Score = 0 });
            var quizInfo = new
            {
                QuizTitle = _quizManager.GetQuizRunner(lobbyCode).Quiz.Title,
                UsersScores = quizRunner.UserScores
            };
            quizInfo.UsersScores.OrderByDescending(us => us.Username);
            Groups.AddToGroupAsync(Context.ConnectionId, lobbyCode);
            Clients.Caller.SendAsync("initalizeQuiz", quizInfo);
            Clients.GroupExcept(lobbyCode, Context.ConnectionId).SendAsync("updateScoreboard", quizInfo.UsersScores);

            if(quizLobby.UsersConnectedAtStart == quizRunner.UserScores.Count)
            {
               return BeginQuiz(lobbyCode);
            }

            return Task.CompletedTask;
        }

        public async Task GetQuestion()
        {
            if (Context.Items.TryGetValue(QuizContextItems.LobbyCode, out var lobbyCode))
            {
                var quizRuuner = _quizManager.GetQuizRunner((string)lobbyCode);
                await Clients.Caller.SendAsync("loadQuestion", quizRuuner.GetQuestion(Context.User.Identity.Name));
            }
            else
                await Clients.Group((string)lobbyCode).SendAsync("displayError", "Connection Lost");
        }

        public async Task ProcessAnswers(int[] answers) 
        {
            if (Context.Items.TryGetValue(QuizContextItems.LobbyCode, out var lobbyCode))
            {
                var quizRunner = _quizManager.GetQuizRunner((string)lobbyCode);
                
                quizRunner.CalculatePoints(Context.User.Identity.Name, answers);

                if (quizRunner.UserScores.All(us => us.IsModiefied))
                {
                    await Clients.Group((string)lobbyCode).SendAsync("updateScoreboard", quizRunner.UserScores);

                    quizRunner.UserScores.ForEach(us => us.IsModiefied = false);

                    if (quizRunner.IsFinished)
                    {
                        await Clients.Group((string)lobbyCode).SendAsync("redirectToSummary");
                    }
                    else
                        await Clients.Group((string)lobbyCode).SendAsync("requestQuestion");
                }
            }
            else
                await Clients.Group((string)lobbyCode).SendAsync("displayError", "Connection Lost");
        } 

        private async Task BeginQuiz(string lobbyCode)
        {
            try
            {
                var quizRunner = _quizManager.GetQuizRunner(lobbyCode);
                var quiz = _quizManager.GetQuizRunner(lobbyCode).Quiz;

                quizRunner.PrepareQuizForUsers(quiz);

                await Clients.Group(lobbyCode).SendAsync("beginQuiz");
            }catch(Exception e)
            {
                await Clients.Group((string)lobbyCode).SendAsync("displayError", "Connection Lost");
            }
        } 

    }
}