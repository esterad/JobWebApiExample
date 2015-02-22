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
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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


                using (var client = new HttpClient())
                {
                    // New code:
                    client.BaseAddress = new Uri(@"http://localhost:56060");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    JobCompletedDTO jobCompleted = new JobCompletedDTO() { Id = Guid.NewGuid(), secret = "test" };
                    HttpResponseMessage response = null;
                    try
                    {
                        response = client.PutAsJsonAsync("api/JobComplete", jobCompleted).Result;
                    }
                    catch (Exception ex)
                    {
                        
                        if (ex is AggregateException)
                        {
                            var listOfExeptions = (ex as AggregateException).InnerExceptions;
                            
                            foreach (var item in listOfExeptions)
                            {
                                Exception current = item;
                                Console.WriteLine(current.Message);
                                do
                                {
                                    
                                    if (current.InnerException!=null)
                                    {
                                        current = current.InnerException;
                                        Console.WriteLine(current.Message);
                                    }
                                } while (current.InnerException != null);
                            }

                        }
                        else
                        {
                            Console.WriteLine(ex.GetBaseException().Message);
                        }

                    }
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        Boolean resp = response.Content.ReadAsAsync<Boolean>().Result;
                    }
                }
                Console.ReadLine();
            }

        }
        public class JobCompletedDTO
        {
            public Guid Id { get; set; }
            public string secret { get; set; }

        }

        public class JobErrorDTO
        {
            public Guid Id { get; set; }
            public string message { get; set; }
            public string secret { get; set; }

        }
    }
}
