using QuizApp.Data.Implementations;
using System.Collections.Generic;

namespace QuizApp.Models.ViewModels
{
    public class SummaryVM
    {
        public List<UserScore> UserScores { get; set; }

        public string LobbyCode { get; set; }

        public int QuizId { get; set; }
    }
}
