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

        [Queue("emails")]
        Task<string> SendReminders();

        [Queue("emails")]
        Task<string> SendWeeklyReport();

        [Queue("emails")]
        Task<string> SendAlerts();

        [Queue("emails")]
        Task<string> SendMonthlyDocumentsReportSMS();

        [Queue("emails")]
        Task<string> SendMonthlyReportEmail();

        [Queue("emails")]
        Task<string> ReserveDocuBoxLockers();

        [Queue("emails")]
        Task<string> ReserveDocuBoxLockersV2();

        [Queue("emails")]
        Task<string> UpdateClosedLockerDate();

        [Queue("emails")]
        Task<string> UpdateClosedLockerDateV2();

        [Queue("emails")]
        Task<string> SendWeeklySalesReportEmail();

        [Queue("emails")]
        Task<string> SyncROeFactura();

        [Queue("emails")]
        Task<string> GetBTTransactions();
    }
}
