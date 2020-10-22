using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;

namespace TNTManagerWebCronJobs
{
    public interface IHangfireEmailJob
    {
        [Queue("emails")] //Specifies that the job only runs on this server's queue. The queue attribute must be in the interface, not the implementation

        Task<string> SendDailyReport();

        Task<string> SendReminders();

        Task<string> SendWeeklyReport();

        Task<string> SendAlerts();
    }
}
