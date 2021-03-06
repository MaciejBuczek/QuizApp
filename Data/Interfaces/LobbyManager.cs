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
        private List<Lobby> _lobbies = new List<Lobby>();

        public void AddLobby(Lobby lobby)
        {
            _lobbies.Add(lobby);
        }

        public void GetLobby(string code)
        {
            throw new NotImplementedException();
        }

        public void RemoveLobby(string code)
        {
            throw new NotImplementedException();
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
