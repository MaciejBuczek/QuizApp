using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models.ViewModels
{
    public class LobbyVM
    {
        public Quiz Quiz { get; set; }
        public string LobbyCode { get; set; }
        public bool IsOwner  { get; set; }
    }
}
