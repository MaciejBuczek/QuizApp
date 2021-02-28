using Microsoft.AspNetCore.Mvc;
using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Controllers
{
    public class XMLInvoiceGeneration
    {
        public Int64 XmlOid { get; set; }
        public string CourierCompany { get; set; }
        public string CourierService { get; set; }
        public Int64? StoreId { get; set; }
    }

    public class QuizController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Quiz quiz)
        {
            return View();
        }
    }
}
