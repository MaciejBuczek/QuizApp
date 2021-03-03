using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models.ViewModels
{
    public class QuestionVM
    {
        public string Content { get; set; }
        public int Time { get; set; }
        public double Points { get; set; }
        public int CorrectAnswer { get; set; }
        public List<AnswerVM> Answers { get; set; }

        public void SetAnswers()
        {
            for (int i = Answers.Count - 1; i >= 0; i--)
            {
                var power = (int)Math.Pow(2, i);
                if (CorrectAnswer >= power)
                {
                    Answers[i].IsCorrect = true;
                    CorrectAnswer -= power;
                }
            }
        }
    }
}
