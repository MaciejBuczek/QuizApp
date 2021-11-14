using QuizApp.Data.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models.ViewModels
{
    public class SummaryVM
    {
        public List<UserScore> UserScores { get; set; }

        public string LobbyCode { get; set; }
    }
}
