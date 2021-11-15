﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Content { get; set; }

        [Display(Name = "Quiz")]
        public int IdQuiz { get; set; }

        [ForeignKey("IdQuiz")]
        public virtual Quiz Quiz { get; set; }


    }
}
