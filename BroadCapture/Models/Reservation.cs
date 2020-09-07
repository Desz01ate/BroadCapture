using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroadCapture.Models
{
    public partial class Reservation
    {
        public long OwnerId { get; set; }
        public string Keyword { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Expired => !(DateTime.Now <= CreateDate.AddMinutes(ExpireInMinute));
        public int ExpireInMinute { get; set; }
    }
}
