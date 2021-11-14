using QuizApp.Data.Implementations;
using QuizApp.Models;

namespace QuizApp.Data.Interfaces
{
    public interface IQuizManager
    {
        public void AddQuiz(Lobby lobby, QuizRunner quizRunner, string code);
        public void RemoveQuiz(string code);
        public Lobby GetLobby(string code);
        public QuizRunner GetQuizRunner(string code);
        public string GetQuizCode();
    } 
}
