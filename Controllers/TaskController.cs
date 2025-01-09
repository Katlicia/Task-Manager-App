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
            var tasks = _context.Tasks
                                .Include(t => t.TaskUsers)
                                .Where(t => t.UserId == userId || t.TaskUsers.Any(tu => tu.UserId == userId))
                                .ToList();

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
                    TempData["SuccessMessage"] = $"{task.Title} task created successfully.";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {

                    ModelState.AddModelError("", "An error occured while creating a task.");
                    return View(model);
                }
            }

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            var model = new TaskViewModel
            {
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted
            };

            ViewData["TaskId"] = task.Id;

            return View(model);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, TaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
                if (task == null)
                {
                    return NotFound();
                }

                task.Title = model.Title;
                task.Description = model.Description;
                task.DueDate = model.DueDate;
                task.IsCompleted = model.IsCompleted;

                try
                {
                    _context.Tasks.Update(task);
                    _context.SaveChanges();
                    TempData["Success Message"] = $"{task.Title} task updated successfully.";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "An error occured while updating the task.");
                }
            }

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var task = _context.Tasks.Include(t => t.TaskUsers).FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            _context.TaskUsers.RemoveRange(task.TaskUsers);

            _context.Tasks.Remove(task);

            try
            {
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Task deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the task.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Task");
        }



        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            try
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
                TempData["SuccessMessage"] = $"{task.Title} task deleted successfully.";
            }

            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = $"An error occured while deleting the task.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var task = _context.Tasks
                .Include(t => t.User)
                .Include(t => t.TaskUsers)
                    .ThenInclude(tu => tu.User)
                .FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            ViewBag.Users = _context.UserAccounts.ToList();

            return View(task);
        }

        [HttpPost]
        public IActionResult MarkAsCompleted(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                TempData["ErrorMessage"] = "Task not found.";
                return RedirectToAction("Index");
            }

            task.IsCompleted = true;
            try
            {
                _context.Tasks.Update(task);
                _context.SaveChanges();
                TempData["SuccessMessage"] = $"{task.Title} marked as completed.";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the task.";
            }

            return RedirectToAction("Index");
        }


        public IActionResult FilterByDueDate(DateTime? dueDate)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var tasks = _context.Tasks.Where(t => t.UserId == userId);

            if (dueDate.HasValue)
            {
                tasks = tasks.Where(t => t.DueDate == dueDate.Value.Date);
                ViewData["SelectedDate"] = dueDate.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewData["SelectedDate"] = "";
            }

            return View("Index", tasks.ToList());
        }

        [HttpPost]
        public IActionResult AssignUserToTask(int taskId, string email)
        {
            var task = _context.Tasks.Include(t => t.TaskUsers).FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                TempData["ErrorMessage"] = "Task not found.";
                return RedirectToAction("Details", new { id = taskId });
            }

            var user = _context.UserAccounts.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User with the provided email does not exist.";
                return RedirectToAction("Details", new { id = taskId });
            }

            if (task.TaskUsers.Any(tu => tu.UserId == user.Id))
            {
                TempData["ErrorMessage"] = "User is already assigned to this task.";
                return RedirectToAction("Details", new { id = taskId });
            }

            var taskUser = new TaskUser
            {
                TaskId = taskId,
                UserId = user.Id,
                CanEdit = true
            };
            _context.TaskUsers.Add(taskUser);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "User assigned to the task successfully.";
            return RedirectToAction("Details", new { id = taskId });
        }



    }
}
