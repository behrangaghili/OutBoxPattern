using OrderApplicaion.Models;

namespace OrderApplicaion.Contract
{
    public interface IOrderOutboxRepository
    {
        Task<IEnumerable<OutboxEventModel>> GetUnpublishedEventsAsync();
        Task AddAsync(OutboxEventModel outboxEvent);
        Task UpdateAsync(OutboxEventModel outboxEvent);
        Task DeleteAsync(Guid eventId);
        Task SaveChangesAsync();
    }

}
