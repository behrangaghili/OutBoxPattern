namespace NotificationMicroservice.EventProcessors
{
    public class EventProcessorFactory
    {
        private readonly List<IEventProcessor> _processors;

        public EventProcessorFactory(List<IEventProcessor> processors)
        {
            _processors = processors;
        }

        public IEventProcessor Create(string eventType)
        {
            return _processors.Where(x=>x.EventType == eventType).SingleOrDefault();
        }
    }
}
