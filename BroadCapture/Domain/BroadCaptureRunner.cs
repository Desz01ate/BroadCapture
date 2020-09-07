using BroadCapture.Domain.Event;
using BroadCapture.Domain.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BroadCapture.Domain
{
    public class BroadCaptureRunner : IBroadCapture
    {
        public event BroadEvent.BroadCapturedEventHandler OnBroadCaptured;
        public event BroadEvent.MaintenanceModeActivatedEventHandler OnMaintenanceModeActivated;
        public event Action<Exception> OnError;
        private readonly GameEngineObservator gameEngineObservator;
        private string latestMessage = string.Empty;
        private DateTime lastUpdate = DateTime.Now;
        public BroadCaptureRunner()
        {
            this.gameEngineObservator = new GameEngineObservator();
        }
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (gameEngineObservator.TryReadBroadMessage(out var broadMessage))
                    {
                        if (latestMessage != broadMessage)
                        {
                            OnBroadCaptured?.Invoke(broadMessage);
                            latestMessage = broadMessage;
                            lastUpdate = DateTime.Now;
                        }
                    }
                    if (DateTime.Now.Subtract(lastUpdate).TotalMinutes >= 10)
                    {
                        OnMaintenanceModeActivated?.Invoke();
                        await Task.Delay(1000);
                    }
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(ex);
                }
                await Task.Delay(Config.Instance.Interval);
            }
        }
    }
}
