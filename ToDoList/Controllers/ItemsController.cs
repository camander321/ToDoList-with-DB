using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System;

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
          
          int categoryId = int.Parse(Request.Form["category"]);
          Category category = Category.Find(categoryId);

          Item newItem = new Item (Request.Form["new-description"], parsedDueDate);
          newItem.Save();
          newItem.AddCategory(category);

          List<Item> allItems = Item.GetAll();
          return View("~/Views/Home/Index.cshtml", allItems);
        }

        [HttpGet("/items/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
          Item thisItem = Item.Find(id);
          return View(thisItem);
        }

        [HttpPost("/items/{id}/update")]
        public ActionResult Update(int id)
        {
          Item thisItem = Item.Find(id);
          thisItem.Edit(Request.Form["newname"]);
          return RedirectToAction("Index");
        }

        [HttpGet("/items/{id}/duedateupdate")]
        public ActionResult UpdateDueDateForm(int id)
        {
          Item thisItem = Item.Find(id);
          return View(thisItem);
        }

        [HttpPost("/items/{id}/duedateupdate")]
        public ActionResult UpdateDueDate(int id)
        {
          Item thisItem = Item.Find(id);
          System.DateTime parsedDueDate = System.DateTime.Parse(Request.Form["newDueDate"]);
          thisItem.EditDueDate(parsedDueDate);
          return RedirectToAction("Index");
        }

        [HttpPost("/items/delete")]
        public ActionResult DeleteAll()
        {
          Item.DeleteAll();
          Item.ResetId();
          Category.DeleteAll();
          Category.ResetId();
          return View();
        }

        [HttpGet("/items/{id}/delete")]
        public ActionResult DeleteItem(int id)
        {
          Item thisItem = Item.Find(id);
          return View(thisItem);
        }

        [HttpPost("/items/{id}/delete")]
        public ActionResult DeleteOne(int id)
        {
          Item deletedItem = Item.Find(id);
          int deletedId = deletedItem.GetId();
          deletedItem.DeleteItem(deletedId);

          List<Item> allItems = Item.GetAll();
          return View("~/Views/Home/Index.cshtml", allItems);
        }

        [HttpPost("/filter/dueDate")]
        public ActionResult DueDate()
        {
          List<Item> filteredList = Item.FilterDueDate();
          return View("~/Views/Home/Index.cshtml", filteredList);
        }
    }
}
