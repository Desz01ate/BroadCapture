using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroadCapture.Domain.Event
{
    public static class BroadEvent
    {
        public delegate void BroadCapturedEventHandler(string broadMessage);
        public delegate void MaintenanceModeActivatedEventHandler();
    }
}
