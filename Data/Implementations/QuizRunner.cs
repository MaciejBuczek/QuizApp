using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Data.Implementations
{
    public class QuizRunner
    {
        public int QuestionCounter { get; set; }

        public List<UserScore> UserScores { get; set; }

        public Dictionary<string, List<ShuffledAnswers>> PersonalisedAnswers { get; set; }

        public QuizRunner()
        {
            QuestionCounter = 0;
            UserScores = new List<UserScore>();
            PersonalisedAnswers = new Dictionary<string, List<ShuffledAnswers>>();
        }

        private List<AnswerCheck> ParseAnswers(List<Answer> answers, int correctAnswer)
        {
            var answersCheck = answers.Select(a => new AnswerCheck
            {
                Answer = a.Content,
                IsCorrect = false
            }).ToList();

            for (int i = answersCheck.Count - 1; i >= 0; i--)
            {
                var pow = (int)Math.Pow(2, i);
                if (correctAnswer >= pow)
                {
                    answersCheck[i].IsCorrect = true;
                    correctAnswer -= pow;
                    if (correctAnswer == 0)
                        return answersCheck;
                }
            }

            return answersCheck;
        }

        private ShuffledAnswers ShuffleAnswers(List<Answer> answers, int correctAnswer)
        {
            var answersCheck = ParseAnswers(answers, correctAnswer);

            var random = new Random();
            var answersShuffled = answersCheck.OrderBy(a => random.Next()).ToList();
            var correctAnswers = new List<int>();

            for (int i = 0; i < answers.Count; i++)
            {
                if (answersShuffled[i].IsCorrect)
                    correctAnswers.Add(i);
            }

            var shuffledAnswers = new ShuffledAnswers
            {
                AnswersContents = answersShuffled.Select(a => a.Answer).ToList(),
                CorrectAnswers = correctAnswers
            };

            return shuffledAnswers;
        }

        public void PrepareQuizForUsers(Quiz quiz)
        {
            if (quiz is null)
                throw new ArgumentNullException(nameof(quiz));

            foreach (var user in UserScores.Select(u => u.Username))
            {

                var shuffledAnswers = new List<ShuffledAnswers>();

                foreach (var question in quiz.Questions)
                {
                    shuffledAnswers.Add(ShuffleAnswers(question.Answers.ToList(), question.CorrectAnswer));
                }

                PersonalisedAnswers.Add(user, shuffledAnswers);
            }
        }
    }
}