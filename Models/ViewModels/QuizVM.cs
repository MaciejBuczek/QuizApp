using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models.ViewModels
{
    public class QuizVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool NegativePoints { get; set; }
        public bool PartialPoints { get; set; }
        public List<QuestionVM> Questions { get; set; }
    }
}
