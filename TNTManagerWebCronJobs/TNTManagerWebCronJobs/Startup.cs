using Autofac;
using Hangfire;
using Owin;
using System;
using System.Collections.Generic;
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
            RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.SendDailyReport(), "0 7 * * 1-5", TimeZoneInfo.Local);
            RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.SendWeeklyReport(), "0 7 * * 1", TimeZoneInfo.Local);
            RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.SendReminders(), "0 * * * *", TimeZoneInfo.Local);

            RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.SendAlerts(), "2 0 * * *", TimeZoneInfo.Local);

            RecurringJob.AddOrUpdate<IHangfireEmailJob>((x) => x.SendMonthlyDocumentsReportSMS(), "*/15 * * * *", TimeZoneInfo.Local);
        }
    }
}