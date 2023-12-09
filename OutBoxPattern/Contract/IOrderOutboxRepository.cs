using OutBoxPattern.Models;

namespace OutBoxPattern.Contract
{
    public interface IOrderOutboxRepository
    {
        Task<IEnumerable<OutboxEvent>> GetUnpublishedEventsAsync();
        Task AddAsync(OutboxEvent outboxEvent);
        Task UpdateAsync(OutboxEvent outboxEvent);
        Task DeleteAsync(Guid eventId);
        Task SaveChangesAsync();
    }

}
