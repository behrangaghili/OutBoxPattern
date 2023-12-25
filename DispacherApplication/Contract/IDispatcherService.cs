public interface IDispatcherService
{
    Task DispatchPendingEventsAsync(CancellationToken cancellationToken);
}
