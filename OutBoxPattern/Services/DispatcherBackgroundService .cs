public class DispatcherBackgroundService : BackgroundService, IDispatcherService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await DispatchPendingEventsAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromMilliseconds(100), stoppingToken); // Example delay
        }
    }

    public async Task DispatchPendingEventsAsync(CancellationToken cancellationToken)
    {
        // Logic to dispatch pending events
    }
}
