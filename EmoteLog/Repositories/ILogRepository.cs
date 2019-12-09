using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmoteLog.Models;

namespace EmoteLog.Repositories
{
    public interface ILogRepository
    {
        IQueryable<LogEntry> Entries { get; }

        void AddEntry(LogEntry entry);
    }
}
