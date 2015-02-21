using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;

namespace OwinSelfhostSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                List<string> filesToReamp = new List<string>()
                    {
                        @"C:\Users\Sławomir\Source\Repos\PsegOnline\plik.wav",
                        @"C:\Users\Sławomir\Source\Repos\PsegOnline\plik2.wav"
                    };
                var job = new JobDTO()
                {
                    Id = Guid.NewGuid(),
                    Command = CommandType.Start,
                    Type = JobType.Reamp,
                    Files = filesToReamp
                };
                using (var client = new HttpClient())
                {

                    var response = client.PostAsJsonAsync(baseAddress + "api/values", job).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(response);
                        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                    }
                }

                using (var client = new HttpClient())
                {
                    // New code:
                    client.BaseAddress = new Uri(baseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync("api/values/1").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        JobDTO job2 = response.Content.ReadAsAsync<JobDTO>().Result;
                    }
                }
                Console.ReadLine();
            }

        }
    }
}
