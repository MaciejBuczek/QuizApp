using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set;}

        [Display(Name ="Question")]
        public int IdQuestion { get; set; }

        [ForeignKey("IdQuestion")]
        public virtual Question Question { get; set; }
    }
}
