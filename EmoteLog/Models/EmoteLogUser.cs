using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace EmoteLog.Models
{
    public class EmoteLogUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedAccount { get; set; }

        public List<LogEntry> UserLog { get; set; }

        public int AverageMood()
        {
            int ratingSum = 0;
            foreach (LogEntry L in UserLog)
            {
                ratingSum += L.Mood;
            }
            return ratingSum / UserLog.Count;
        }

        public int IntervalAverageMood(DateTime start, DateTime end)
        {
            int ratingSum = 0;
            int logCount = 0;
            foreach (LogEntry L in UserLog)
            {
                if (L.PublishDate >= start && L.PublishDate <= end)
                {
                    ratingSum += L.Mood;
                    logCount++;
                }
            }
            return ratingSum / logCount;
        }

    }
}
