using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bronik.Controllers
{
    public class HomeController : Controller
    {
        private Worker worker=new Worker();
        [Route("/")]
        public IActionResult Index()
        {
            ViewBag.Tables = worker.GetTables();
            return View("~/Views/Home/index.cshtml");
        }
        [Route("/Book")]
        public IActionResult Book(int id,string Name, int number,string tel)
        {
            worker.OpenDesk(id, Name, number, tel,DateTime.Now);
            return Index();
        }
    }
}