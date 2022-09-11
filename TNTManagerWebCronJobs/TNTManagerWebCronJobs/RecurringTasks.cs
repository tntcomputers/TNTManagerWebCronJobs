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
        //public static string DEV_PATH { get; } = "http://localhost:64912/";
        public static string API_PATH { get; } = "https://api.citymanager.ro/";

        public static HttpClient ApiClient { get; set; }

        private static void InitializeHTTPClient()
        {
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri(DEV_PATH);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //ApiClient.DefaultRequestHeaders.Add("Authorization", "19693aa9131fbcee37dbbdd759b72a12");
            ApiClient.DefaultRequestHeaders.Add("Authorization", "87e7db6abf92a4a5ac1a5f2482893603");
            
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

        public async Task<string> UpdateClosedLockerDate()
        {
            var authHttpClient = new HttpClient()
            {
                BaseAddress = new Uri(API_PATH),
                Timeout = TimeSpan.FromMinutes(3600)
            };
            authHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            var authResponse = authHttpClient.PostAsync("api/authorize/token/W2naBtVkcVFF2Z2plQUJNUE93Q3ljT3FhaDVFUXU1eXlWanBtVkc", new StringContent("")).Result;

            var token = await authResponse.Content.ReadAsStringAsync();
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(API_PATH),
                Timeout = TimeSpan.FromMinutes(3600)
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = httpClient.PostAsync("api/DocuBox/UpdateClosedLockerDate", new StringContent("")).Result;

            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<string> SendWeeklySalesReportEmail()
        {
            InitializeHTTPClient();

            var response = ApiClient.PostAsync("api/Hangfire/SendWeeklySalesReport", new StringContent("")).Result;
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SyncROeFactura()
        {
            InitializeHTTPClient();

            var response = ApiClient.PostAsync("api/Hangfire/SyncROeFactura", new StringContent("")).Result;
            return await response.Content.ReadAsStringAsync();
        }
    }
}