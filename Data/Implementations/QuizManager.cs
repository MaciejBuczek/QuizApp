using QuizApp.Data.Implementations;
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Data.Interfaces
{
    public class QuizManager : IQuizManager
    {
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int _codeLength = 6;
        private readonly Random _random;

        private Dictionary<string, (Lobby Lobby, QuizRunner QuizRunner)> _quizDic;

        public QuizManager()
        {
            _quizDic = new Dictionary<string, (Lobby Lobby, QuizRunner QuizRunner)>();
            _random = new Random();
        }

        public void AddQuiz(Lobby lobby, QuizRunner quizRunner, string code)
        {
            _quizDic.Add(code, (lobby, quizRunner));
        }

        public Lobby GetLobby(string code)
        {
            if(_quizDic.TryGetValue(code, out var tuple))
            {
                return tuple.Lobby;
            }
            return null;
        }

        public string GetQuizCode()
        {
            string code;
            do
            {
                code = new string(Enumerable.Repeat(_chars, _codeLength).Select(s => s[_random.Next(s.Length)]).ToArray());
            } while (_quizDic.Keys.Any(k => k == code));
            return code;
        }

        public QuizRunner GetQuizRunner(string code)
        {
            return _quizDic[code].QuizRunner;
        }

        public void RemoveQuiz(string code)
        {
            _quizDic.Remove(code);
        }

        public string RegenerateCode(string previousCode)
        {
            var newCode = GetQuizCode();
            var quiz = _quizDic[previousCode];

            _quizDic.Remove(previousCode);
            _quizDic.Add(newCode, quiz);

            return newCode;
        }
    }
}
