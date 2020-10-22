using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hangfire;

namespace CronInterface
{
    public interface IHangfireEmailJob
    {
        [Queue("qa_emails")] //Specifies that the job only runs on this server's queue. The queue attribute must be in the interface, not the implementation.

        Task<string> SendDailyReport();
    }
}
