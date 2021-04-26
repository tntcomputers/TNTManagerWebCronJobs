using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace TNTManagerWebCronJobs
{
    public class RecurringTasks : IHangfireEmailJob
    {
        public static string DEV_PATH { get; } = "https://dev.tntsoftware.ro/";

        public static HttpClient ApiClient { get; set; }

        private static void InitializeHTTPClient()
        {
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri(DEV_PATH);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ApiClient.DefaultRequestHeaders.Add("Authorization", "19693aa9131fbcee37dbbdd759b72a12");
            ApiClient.Timeout = TimeSpan.FromMinutes(3600);
        }

        public async Task<string> SendDailyReport()
        {
            InitializeHTTPClient();

            //using (SqlCommand cmd = new SqlCommand())
            //{
            //    cmd.Connection = new SqlConnection(ConfigurareConexiuneBD.CS);
            //    cmd.Connection.Open();
            //    cmd.CommandText = query;
            //    //List<AlertaParametriValori> parametriValori = sablon.AlertaParametriValori.ToList();
            //    //foreach (var p in tip.AlertaParametri)
            //    //{
            //    //    cmd.Parameters.AddWithValue(p.Denumire.Replace(' ', '_'), parametriValori.FirstOrDefault(valoare => valoare.ParametruId == p.Id).Valoare);
            //    //}


            //    SqlDataReader reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    { }
            //}



            var response = ApiClient.PostAsync("api/Hangfire/SendDailyReport", new StringContent("")).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SendWeeklyReport()
        {
            InitializeHTTPClient();

            var response = ApiClient.PostAsync("api/Hangfire/SendWeeklyReport", new StringContent("")).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SendReminders()
        {
            InitializeHTTPClient();

            var response = ApiClient.PostAsync("api/Hangfire/SendTestReminders", new StringContent("")).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SendAlerts()
        {
            InitializeHTTPClient();

            var response = ApiClient.PostAsync("api/Hangfire/SendAlerts", new StringContent("")).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SendMonthlyDocumentsReportSMS()
        {
            InitializeHTTPClient();

            var response = ApiClient.PostAsync("api/Hangfire/SendMonthlyDocumentsReportSMS", new StringContent("")).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SendMonthlyReportEmail()
        {
            InitializeHTTPClient();

            var response = ApiClient.PostAsync("api/Hangfire/SendMonthlyReportEmail", new StringContent("")).Result;
            return await response.Content.ReadAsStringAsync();

        }

        public async Task<string> ReserveDocuBoxLockers()
        {
            InitializeHTTPClient();

            var response = ApiClient.PostAsync("api/Hangfire/ReserveLockersAsync", new StringContent("")).Result;
            return await response.Content.ReadAsStringAsync();
        }
    }
}