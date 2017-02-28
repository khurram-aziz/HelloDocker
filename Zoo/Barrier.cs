using org.apache.zookeeper;
using org.apache.zookeeper.data;
using System;
using System.Threading.Tasks;

namespace Zoo
{
    class Barrier
    {
        string rootNode = @"/dotnetcoreapp";
        string barrierNode = @"/dotnetcoreapp/barrier";
        string participantsNode = @"/dotnetcoreapp/barrier/participants";
        string reachedNode = @"/dotnetcoreapp/barrier/reached";

        Watcher watcher = NullWatcher.Instance;
        ZooKeeper zoo;

        private Barrier()
        {
        }

        public Barrier(string connectionString) : this()
        {
            this.zoo = new ZooKeeper(connectionString, 5000, this.watcher);
        }

        async Task<object> createNodeAsync(string nodePath, CreateMode mode)
        {
            Stat stat = await this.zoo.existsAsync(nodePath, watch: false);
            if (null == stat)
                return await this.zoo.createAsync(nodePath, new byte[0],
                    ZooDefs.Ids.OPEN_ACL_UNSAFE, mode);
            else
                return await Task.FromResult(true);
        }

        public async Task<bool> EnrollIntoBarrierAsync(TimeSpan timeToRoll, string participantName)
        {
            await this.createNodeAsync(this.rootNode, CreateMode.PERSISTENT);
            await this.createNodeAsync(this.barrierNode, CreateMode.PERSISTENT);
            await this.createNodeAsync(this.participantsNode, CreateMode.PERSISTENT);
            await this.createNodeAsync(this.reachedNode, CreateMode.PERSISTENT);

            string completeNode = this.barrierNode + "/rollcomplete";
            
            Stat stat = await this.zoo.existsAsync(completeNode, watch: false);
            if (null == stat)
            {
                string thisNode = this.participantsNode + @"/" + participantName;
                await this.createNodeAsync(thisNode, CreateMode.EPHEMERAL);

                await Task.Delay(timeToRoll); //lets wait for other nodes to join
                await this.createNodeAsync(completeNode, CreateMode.EPHEMERAL);
                
                //now chances are that this node is still late to roll
                Stat statComplete = await this.zoo.existsAsync(completeNode, watch: false);
                Stat statMyNode = await this.zoo.existsAsync(thisNode, watch: false);
                if (statMyNode.getCzxid() < statComplete.getCzxid())
                    return true; //this node was enrolled
                else
                    await this.zoo.deleteAsync(thisNode);
            }
            return false;
        }

        public async Task<int> GetParticipantCountAsync()
        {
            var children = await this.zoo.getChildrenAsync(this.participantsNode);
            return children.Stat.getNumChildren(); // unenrolled nodes can still exist
            // we should check czxid of children against barrier/rollcomplete
        }

        public async Task<object> ReachBarrierAsync(string participantName)
        {
            return await this.createNodeAsync(this.reachedNode + "/" + participantName, CreateMode.EPHEMERAL);
        }

        public void WaitAtBarrier(int participantCount)
        {
            while (this.zoo.getChildrenAsync(this.reachedNode, this.watcher).Result.Stat.getNumChildren() != participantCount)
                ;
        }
    }
}
