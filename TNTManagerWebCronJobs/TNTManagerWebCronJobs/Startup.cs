using Autofac;
using Hangfire;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TNTManagerWebCronJobs
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Build Autofac IoC container
            var builder = new ContainerBuilder();

            //Link our jobs interface to its implementation
            builder.RegisterType<RecurringTasks>().As<IHangfireEmailJob>().AsImplementedInterfaces().InstancePerBackgroundJob();
            var container = builder.Build();
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("TNTRegisConnectionString") //The connection string for our hangfire database, defined in web.config
                .UseAutofacActivator(container);

            //This server only processes jobs from this queue
            app.UseHangfireServer(new BackgroundJobServerOptions() { Queues = new string[] { "emails" } });

            //GlobalConfiguration.Configuration.UseSimpleAssemblyNameTypeSerializer().UseRecommendedSerializerSettings().UseSqlServerStorage("TNTRegisConnectionString");
            app.UseHangfireDashboard();

            //Autofac will instantiate the class implementing this interface. The dashboard knows about the interface and can thus display the job properly.
            char[] separator = { ',' };
            string[] jobs = ConfigurationManager.AppSettings["jobs"].Split(separator, options: StringSplitOptions.RemoveEmptyEntries);

            if (jobs.Contains("roefactura"))
            {
                RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.SyncROeFactura(), "10 7-18 * * 1-5", TimeZoneInfo.Local);
            }

            if (jobs.Contains("registraturaReports"))
            {
                RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.SendDailyReport(), "0 8 * * 1-5", TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.SendWeeklyReport(), "0 8 * * 1", TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.SendMonthlyDocumentsReportSMS(), "0 9 1-5 * *", TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.SendMonthlyReportEmail(), "0 */15 * ? * *", TimeZoneInfo.Local);
            }

            if (jobs.Contains("docubox"))
            {
                RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.ReserveDocuBoxLockers(), "*/10 * * * *", TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.UpdateClosedLockerDate(), "*/2 * * * *", TimeZoneInfo.Local);
            }

            if (jobs.Contains("docuboxV2"))
            {
                RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.ReserveDocuBoxLockersV2(), "*/10 * * * *", TimeZoneInfo.Local);
                RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.UpdateClosedLockerDateV2(), "*/2 * * * *", TimeZoneInfo.Local);
            }

            if (jobs.Contains("alerts"))
            {
                RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.SendAlerts(), "28 11 * * *", TimeZoneInfo.Local);
            }

            if (jobs.Contains("reminders"))
            {
                RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.SendReminders(), "0 * * * *", TimeZoneInfo.Local);
            }

            if (jobs.Contains("sales"))
            {
                RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.SendWeeklySalesReportEmail(), Cron.Weekly(DayOfWeek.Monday, 8), TimeZoneInfo.Local);
            }

            if (jobs.Contains("bt"))
            {
                RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.GetBTTransactions(), "12 7,18 * * *", TimeZoneInfo.Local);
            }
        }
    }
}