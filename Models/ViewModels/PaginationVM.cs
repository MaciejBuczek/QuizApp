using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models.ViewModels
{
    public class PaginationVM
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
