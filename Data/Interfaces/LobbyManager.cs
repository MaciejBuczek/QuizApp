using QuizApp.Data.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Data.Interfaces
{
    public class LobbyManager : ILobbyManager
    {
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int _codeLength = 6;
        private readonly Random _random = new Random();
        private readonly List<Lobby> _lobbies = new List<Lobby>();

        public void AddLobby(Lobby lobby)
        {
            _lobbies.Add(lobby);
        }

        public Lobby GetLobby(string code)
        {
            return _lobbies.Where(l => l.Code == code).FirstOrDefault();
        }

        public void RemoveLobby(string code)
        {
            _lobbies.Remove(_lobbies.Where(l => l.Code == code).FirstOrDefault());
        }

        public string GetLobbyCode()
        {
            string code;
            do
            {
                code = new string(Enumerable.Repeat(_chars, _codeLength).Select(s => s[_random.Next(s.Length)]).ToArray());
            } while (_lobbies.Any(l => l.Code == code));
            return code;
        }
    }
}
