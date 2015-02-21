using System;
using System.Collections.Generic;
using System.Web.Http;

namespace OwinSelfhostSample
{
    public class ValuesController : ApiController
    {
        // GET api/values 
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5 
        public JobDTO Get(int id)
        {
            List<string> filePathList= new List<string>()
                    {
                        @"C:\Users\Sławomir\Source\Repos\PsegOnline\plik.wav",
                        @"C:\Users\Sławomir\Source\Repos\PsegOnline\plik2.wav"
                    };
            var job = new JobDTO()
            {
                Id = Guid.NewGuid(),
                Command = CommandType.Start,
                Type = JobType.Reamp,
                Files = filePathList
            };
            return job;
        }

        // POST api/values 
        public void Post(JobDTO value)
        {
            var test = value;
            
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }
    }
}