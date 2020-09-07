using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroadCapture.Models
{
    public class BotRequestLog
    {
        public long Uid { get; set; }
        public string Username { get; set; }
        public string CommandType { get; set; }
        public string FullCommand { get; set; }
        public DateTime CreateDate { get; set; }
        public BotRequestLog()
        {
            CreateDate = DateTime.Now;
        }
    }
}
