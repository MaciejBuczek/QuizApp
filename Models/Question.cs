using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        [Required]
        public double Points { get; set; }

        [Required]
        public int CorrectAnswer { get; set; }

        [Display(Name ="Quiz")]
        public int IdQuiz { get; set; }

        [ForeignKey("IdQuiz")]
        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
