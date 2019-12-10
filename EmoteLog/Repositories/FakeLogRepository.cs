using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmoteLog.Models;

namespace EmoteLog.Repositories
{
    public class FakeLogRepository : ILogRepository
    {
        private List<LogEntry> entries = new List<LogEntry>();
        public IQueryable<LogEntry> Entries => entries.AsQueryable();

        public void AddEntry(LogEntry entry)
        {
            entries.Add(entry);
        }
    }
}
