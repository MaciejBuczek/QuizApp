using System.Collections.Generic;

namespace QuizApp.Data.Implementations
{
    public class Lobby
    {
        public string OwnerUsername { get; set; }
        public bool Private { get; set; }
        public List<string> ConnectedUsers { get; set; } = new List<string>();
    }
}
