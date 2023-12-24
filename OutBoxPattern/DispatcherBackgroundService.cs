
namespace DispacherApplication
{
    public class DispatcherBackgroundService : BackgroundService
    {
        private readonly IDispatcherService dispatcherService;

        public DispatcherBackgroundService(IDispatcherService dispatcherService)
        {
            this.dispatcherService = dispatcherService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await dispatcherService.DispatchPendingEventsAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromMilliseconds(500), stoppingToken);
            }
        }
    }
}
