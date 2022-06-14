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
        public IActionResult Book(int id,string name, int number,string tel,DateTime date)
        {
            worker.OpenDesk(id, name, number, tel,date);
            return Index();
        }
        [Route("/Close")]
        public IActionResult Close(int id)
        {
            worker.CloseOrder(id);
            return Orders();
        }
        [Route("/Orders")]
        public IActionResult Orders()
        {
            ViewBag.Tables = worker.GetTables();
            return View("~/Views/Home/Orders.cshtml");
        }
    }
}