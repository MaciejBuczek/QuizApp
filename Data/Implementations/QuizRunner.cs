using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Data.Implementations
{
    public class QuizRunner
    {
        private int _requestCounter;

        private int _questionCounter;


        public List<UserScore> UserScores { get; set; }

        public Dictionary<string, List<ShuffledAnswers>> PersonalisedAnswers { get; set; }

        public Quiz Quiz { get; set; }

        public QuizRunner(Quiz quiz)
        {
            _requestCounter = 0;
            _questionCounter = 0;

            UserScores = new List<UserScore>();
            PersonalisedAnswers = new Dictionary<string, List<ShuffledAnswers>>();
            Quiz = quiz;
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

        public PersonalisedQuestion GetQuestion(string login)
        {
            if (string.IsNullOrEmpty(login))
                throw new ArgumentException(nameof(login));

            var question = Quiz.Questions.ToArray()[_questionCounter];

            var personalisedQuestion = new PersonalisedQuestion
            {
                Question = question.Content,
                Answers = PersonalisedAnswers[login][_questionCounter].AnswersContents,
                Time = question.Time
            };

            if (++_requestCounter == UserScores.Count)
            {
                _questionCounter++;
                _requestCounter = 0;
            }

            return personalisedQuestion;
        }

        public void CalculatePoints(string username, int[] answers)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException(nameof(username));

            var points = Quiz.Questions[_questionCounter - 1].Points;
            var correctAnswers = PersonalisedAnswers[username][_questionCounter - 1].CorrectAnswers;
            var userScore = UserScores.Where(us => us.Username == username).FirstOrDefault();

            userScore.IsModiefied = true;

            if (correctAnswers.SequenceEqual(answers))
                 userScore.Score += points;
            else
            {
                if (Quiz.NegativePoints)
                    userScore.Score -= points;
                else if (Quiz.PartialPoints)
                {
                    var correctAnswersCounter = 0d;

                    foreach (var answer in answers)
                    {
                        if (correctAnswers.Contains(answer))
                            correctAnswersCounter++;
                        else
                            return;
                    }
                    userScore.Score += Math.Round((double)(correctAnswersCounter / correctAnswers.Count) * points, 2);
                }
            }
        }
    }
}