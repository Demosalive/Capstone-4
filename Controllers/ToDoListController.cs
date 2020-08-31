using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Capstone_Third_Time.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Third_Time.Controllers
{
    public class ToDoListController : Controller
    {
        private readonly ToDoListDbContext _todoContext;

        public ToDoListController(ToDoListDbContext todoContext)
        {
            _todoContext = todoContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> ToDoList()
        {
            string activeUserName = User.Identity.Name;
            string activeUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var toDos = await _todoContext.ToDoList.Where(x => x.UserId == activeUserId).ToListAsync();
            ViewBag.UserName = activeUserName;


            return View(toDos);
        }

        [HttpGet]
        public IActionResult AddToDoList()
        {

            return View();
        }
       
        [HttpPost]
        public IActionResult AddToDoList(ToDoList todoList)
        {
            string activeUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (ModelState.IsValid)
            {
                 todoList.UserId = activeUserId;
                _todoContext.ToDoList.Add(todoList);
                _todoContext.SaveChanges();//dont forget this  saves it back to sql
            }

            return RedirectToAction("ToDoList");//RedirectTo Action goes to method index
        }
         
        public IActionResult MarkComplete(int id)
        {
            var foundToDoList = _todoContext.ToDoList.Find(id);
            if (foundToDoList != null)
            {
                foundToDoList.Complete = true;
                _todoContext.SaveChanges();
            }
            return RedirectToAction("ToDoList");
        }

        public IActionResult DeleteToDoList(int id)
        {
            //find the to do list we want to delete from the database
            //Find() will bring back an entire to do list via its primary key
            var foundToDoList = _todoContext.ToDoList.Find(id);
            if (foundToDoList != null)
            {
                _todoContext.ToDoList.Remove(foundToDoList);
                _todoContext.SaveChanges();
            }
            return RedirectToAction("ToDoList");
        }

        [Authorize]
        public async Task<IActionResult> ToDoListId()
        {
            string activeUserName = User.Identity.Name;
            string activeUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var toDos = await _todoContext.ToDoList.Where(x => x.UserId == activeUserId).ToListAsync();
            ViewBag.UserName = activeUserName;

            return View(toDos);
        }

    }
}
