using QuizApp.Views.Lobby;
using System.Collections.Generic;
using System.Linq;

namespace QuizApp.Data.Implementations
{
    public class Lobby
    {
        private Dictionary<string, bool> _connectedUsersFlags = new Dictionary<string, bool>();

        public LobbyStatus LobbyStatus { get; set; }
        public string OwnerUsername { get; set; }
        public string Code { get; set; }
        public bool Private { get; set; }
        public int QuizId { get; set; }
        public List<string> ConnectedUsers { get; set; } = new List<string>();

        public void AddUserFlag(string userId, bool value)
        {
            _connectedUsersFlags.Add(userId, value);
        }

        public void GenerateUserFlags()
        {
            foreach(var user in ConnectedUsers)
            {
                _connectedUsersFlags.Add(user, false);
            }
        }

        public void SetUserFlag(string userId, bool value)
        {
            _connectedUsersFlags[userId] = value;
        }

        public bool AreAllUserFlagsActivated()
        {
            return _connectedUsersFlags.Values.All(v => v);
        }
    }
}
