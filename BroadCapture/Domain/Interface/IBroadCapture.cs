using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BroadCapture.Domain.Interface
{
    public interface IBroadCapture
    {
        event Event.BroadEvent.BroadCapturedEventHandler OnBroadCaptured;
        event Event.BroadEvent.MaintenanceModeActivatedEventHandler OnMaintenanceModeActivated;
        Task RunAsync(CancellationToken cancellationToken);
    }
}
