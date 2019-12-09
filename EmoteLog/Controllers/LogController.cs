using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using EmoteLog.Repositories;
using EmoteLog.Models;
using Microsoft.AspNetCore.Authorization;

namespace EmoteLog.Controllers
{
    [Authorize]
    public class LogController : Controller
    {
        static ILogRepository _logRepo;

        public LogController(ILogRepository logs)
        {
            _logRepo = logs;
        }

        [HttpGet]
        public IActionResult NewEntry(string userId)
        {
            ViewBag.userId = userId;
            return View();
        }

        [HttpPost]
        public IActionResult NewEntry(LogEntry newEntry)
        {
            if (ModelState.IsValid)
            {
                newEntry.PublishDate = DateTime.Now;
                _logRepo.AddEntry(newEntry);
                return RedirectToAction("Profile", "Account");
            }
            else
            {
                //there is a validation error
                return View("NewEntry");
            }
        }

        public int AverageMood(List<LogEntry> currentUserLog)
        {
            int ratingSum = 0;
            foreach (LogEntry entry in currentUserLog)
            {
                ratingSum += entry.Mood;
            }
            return ratingSum / currentUserLog.Count();
        }

        // later implementation
        public int IntervalAverageMood(List<LogEntry> currentUserLog, DateTime start, DateTime end)
        {
            int ratingSum = 0;
            int logCount = 0;
            foreach (LogEntry entry in currentUserLog)
            {
                if (entry.PublishDate >= start && entry.PublishDate <= end)
                {
                    ratingSum += entry.Mood;
                    logCount++;
                }
            }
            return ratingSum / logCount;
        }

        public IActionResult Journal(string userId)
        {
            List<LogEntry> currentUserLog = _logRepo.Entries.Where(e => e.UserId == userId).ToList();
            return View("Journal", currentUserLog);
        }
    }
}