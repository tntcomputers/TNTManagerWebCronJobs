using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace TNTManagerWebCronJobs
{
    public class RecurringTasks : IHangfireEmailJob
    {
        public static string DEV_PATH { get; } = "http://dev.tntsoftware.ro/";

        public static HttpClient ApiClient { get; set; }

        private static void InitializeHTTPClient()
        {
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri(DEV_PATH);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //ApiClient.DefaultRequestHeaders.Add("Authorization", apiKey);
            ApiClient.Timeout = TimeSpan.FromMinutes(3600);
        }

        public async Task<string> SendDailyReport()
        {
            InitializeHTTPClient();

            var response = ApiClient.PostAsync("api/DailyReport", new StringContent("")).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SendWeeklyReport()
        {
            InitializeHTTPClient();

            var response = ApiClient.PostAsync("api/WeeklyReport", new StringContent("")).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SendReminders()
        {
            InitializeHTTPClient();

            var response = ApiClient.PostAsync("api/TestReminders", new StringContent("")).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SendAlerts()
        {
            InitializeHTTPClient();

            var response = ApiClient.PostAsync("api/Alerts", new StringContent("")).Result;
            return await response.Content.ReadAsStringAsync();
        }
    }
}