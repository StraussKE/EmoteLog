using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmoteLog.Models;

namespace EmoteLog.Repositories
{
    public class FakeLogRepository : ILogRepository
    {
        public IQueryable<LogEntry> Entries => new List<LogEntry>
        {
            new LogEntry
            {
                LogId = new Guid(),
                Mood = 5,
                Entry = "I'm feeling fairly flat today.  It's not a good day, it's not a bad day, it's just a day."
            }
            
        }.AsQueryable<LogEntry>();

        public void AddEntry(LogEntry entry)
        {
            Entries.ToList().Add(entry);
        }
    }
}
