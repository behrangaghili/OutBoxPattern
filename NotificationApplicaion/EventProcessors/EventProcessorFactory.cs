namespace NotificationMicroservice.EventProcessors
{
    public class EventProcessorFactory
    {
        private readonly IEnumerable<IEventProcessor> _processors;

        public EventProcessorFactory(IEnumerable<IEventProcessor> processors)
        {
            _processors = processors;
        }

        public IEventProcessor Create(string eventType)
        {
            return _processors.Where(x=>x.EventType == eventType).SingleOrDefault();
        }
    }
}
