using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroadCapture.Models
{
    public class Preferences
    {
        public long UserId { get; set; }
        public int? SearchRange { get; set; } = 10;
        public int? SearchLimit { get; set; } = 10;
    }
}
