using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroadCapture.Models
{
    public class ErrorLog
    {
        public string ErrorDetail { get; set; }
        public DateTime CreateDate { get; set; }
        public ErrorLog()
        {
            CreateDate = DateTime.Now;
        }
        public ErrorLog(string message)
        {
            ErrorDetail = message;
            CreateDate = DateTime.Now;
        }
    }
}
