using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet("/")]
        public ActionResult Index()
        {
            List<Item> allItems = Item.GetAll();
            return View("Index", allItems);
        }
    }
}
