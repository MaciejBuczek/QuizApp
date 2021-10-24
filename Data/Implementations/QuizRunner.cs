using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Data.Implementations
{
    public class QuizRunner
    {
        public Quiz Quiz { get; set; }

        public int QuestionCounter { get; set; }

        public List<(string Username, double Score)> UserScores { get; set; }
    }
}
