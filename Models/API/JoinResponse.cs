using System;

namespace QuizApp.Models.API
{
    public class JoinResponse
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public double Rating { get; set; }

        public int TotalQuestions { get; set; }

        public int TotalTime { get; set; }

        public double TotalPoints { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }
    }
}
