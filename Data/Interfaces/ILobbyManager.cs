using QuizApp.Data.Implementations;

namespace QuizApp.Data.Interfaces
{
    public interface ILobbyManager
    {
        public void AddLobby(Lobby lobby);
        public void RemoveLobby(string code);
        public void GetLobby(string code);
        public string GetLobbyCode();
    }
}
