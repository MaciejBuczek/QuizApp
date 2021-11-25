using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models.ViewModels
{
    public class QuizDisplayVM : PaginationVM
    {
        public List<Quiz> Quizzes { get; set; }
        public string QuizTitle { get; set; }
        public string AuthorUsername { get; set; }
    }
}
