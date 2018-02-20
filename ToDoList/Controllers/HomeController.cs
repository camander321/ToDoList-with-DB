// using System.Collections.Generic;
// using Microsoft.AspNetCore.Mvc;
// using ToDoList.Models;
//
// namespace ToDoList.Controllers
// {
//     public class ItemsController : Controller
//     {
//
//         [HttpGet("/items")]
//         public ActionResult Index()
//         {
//             // Item newItem = new Item(Request.Query["new-item"]);
//             // return View(newItem);
//             List<Item> allItems = Item.GetAll();
//             return View();
//         }
//
//         [HttpGet("/items/new")]
//         public ActionResult CreateForm()
//         {
//             return View();
//         }
//
//         [HttpPost("/items")]
//         public ActionResult Create()
//         {
//           Item newItem = new Item (Request.Form["new-item"]);
//           newItem.Save();
//           return View("Index", newItem);
//         }
//
//         [HttpPost("/items/delete")]
//         public ActionResult DeleteAll()
//         {
//           Item.ClearAll();
//           return View();
//         }
//
//     }
// }
