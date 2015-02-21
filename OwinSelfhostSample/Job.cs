using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwinSelfhostSample
{
    public class JobDTO
    {
        public Guid Id { get; set; }
        public JobType Type { get; set; }
        public CommandType Command { get; set; }
        public double? Start { get; set; } //nullable
        public double Length { get; set; }
        public List<string> Files = new List<string>();

    }
    public enum JobType
    {
        Tune = 1, Reamp = 2, Sum = 3
    }

    public enum CommandType
    {
        Stop = 0,
        Start = 1
    }
    public enum ChannelType
    {
        Stereo = 0, Mono = 1
    }
}
