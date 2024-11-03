﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoApp.Entities;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var tasks = _context.Tasks.Where(t => t.UserId == userId).ToList();
            return View(tasks);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(TaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                Entities.Task task = new Entities.Task();
                task.Title = model.Title;
                task.Description = model.Description;
                task.DueDate = model.DueDate;
                task.IsCompleted = false;
                task.CreatedDate = DateTime.Now;
                task.UserId = userId;

                try
                {
                    _context.Tasks.Add(task);
                    _context.SaveChanges();

                    ModelState.Clear();
                    //ViewBag.Message = $"{task.Title} task created successfully.";
                    TempData["SuccessMessage"] = $"{task.Title} task created successfully.";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {

                    ModelState.AddModelError("", "An error occured while creating a task.");
                    return View(model);
                }
                //return View();
            }

            return View(model);
        }
    }
}
