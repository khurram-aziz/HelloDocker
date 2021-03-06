using System.Threading.Tasks;
using org.apache.zookeeper;

namespace Zoo
{
    class NullWatcher : Watcher
    {
        public static readonly NullWatcher Instance = new NullWatcher();

        private NullWatcher() { }

        public override Task process(WatchedEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}