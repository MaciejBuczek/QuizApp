using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models.ViewModels
{
    public class QuizDisplayVM
    {
        public List<Quiz> Quizzes { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Search { get; set; }
    }
}
