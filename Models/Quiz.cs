using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public bool NegativePoints { get; set; }
        [Required]
        public bool PartialPoints { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser CreatedBy { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
