using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmoteLog.Models;

namespace EmoteLog.Repositories
{
    public class EFLogRepository : ILogRepository
    {
        private AppIdentityDbContext context;

        public EFLogRepository(AppIdentityDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<LogEntry> Entries => context.UserLogs;


        public void AddEntry(LogEntry entry)
        {
            context.Add(entry);
            context.SaveChanges();
        }
    }
}
