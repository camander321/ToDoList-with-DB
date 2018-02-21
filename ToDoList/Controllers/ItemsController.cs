using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;

namespace ToDoList.Controllers
{
    public class ItemsController : Controller
    {

        [HttpGet("/items")]
        public ActionResult Index()
        {
          List<Item> allItems = Item.GetAll();
          return View("~/Views/Home/Index.cshtml", allItems);
        }

        [HttpGet("/items/new")]
        public ActionResult CreateForm()
        {
            return View();
        }

        [HttpPost("/items")]
        public ActionResult Create()
        {
          string newDueDate = Request.Form["new-duedate"];
          System.DateTime parsedDueDate = System.DateTime.Parse(newDueDate);
          Item newItem = new Item (Request.Form["new-description"], parsedDueDate);
          newItem.Save();
          List<Item> allItems = Item.GetAll();
          return View("~/Views/Home/Index.cshtml", allItems);
        }

        [HttpPost("/items/delete")]
        public ActionResult DeleteAll()
        {
            Item.DeleteAll();
            return View();
        }

    }
}
