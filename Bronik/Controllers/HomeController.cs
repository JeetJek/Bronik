using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bronik.Controllers
{
    public class HomeController : Controller
    {
        private Worker worker=new Worker();
        public IActionResult Index()
        {
            ViewBag.Tables = worker.GetTables();
            return View();
        }
        public IActionResult Book(int id,string Name,int Clients, string Num)
        {
            worker.SetTable(id, Name);
            return View("~/Views/Home/Index.cshtml");
        }
    }
}