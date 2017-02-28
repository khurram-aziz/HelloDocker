using System;
using System.Threading;

namespace Zoo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //127.0.0.1:2181,127.0.0.1:2182,127.0.0.1:2183
            //localhost:2181,localhost:2182,localhost:2183/app
            //localhost:2181
            if (args.Length != 4) return;

            Program.DriveToBoston("zoo:2181", args[0],
                TimeSpan.FromSeconds(int.Parse(args[1])),   //timeToLeaveHome
                TimeSpan.FromSeconds(int.Parse(args[2])),   //timeToRoll
                TimeSpan.FromSeconds(int.Parse(args[3]))    //timeToGasStation
            );
        }

        static void DriveToBoston(string connectionString, string name, TimeSpan timeToLeaveHome, TimeSpan timeToRoll, TimeSpan timeToGasStation)
        {
            try
            {
                Console.WriteLine("[{0}] Leaving house", name);
                Thread.Sleep(timeToLeaveHome); //let zookeeper come online and decision time

                var barrier = new Barrier(connectionString);
                bool envrolled = barrier.EnrollIntoBarrierAsync(timeToRoll, name).Result;

                if (!envrolled)
                {
                    Console.Write("[{0}] Couldnt join the caravan!", name);
                    return;
                }

                Console.WriteLine("[{0}] Going to Boston!", name);
                int participants = barrier.GetParticipantCountAsync().Result;
                Console.WriteLine("[{0}] Caravan has {1} cars!", name, participants);

                // Perform some work
                Thread.Sleep(timeToGasStation);
                object o = barrier.ReachBarrierAsync(name).Result;
                Console.WriteLine("[{0}] Arrived at Gas Station", name);

                // Need to sync here
                barrier.WaitAtBarrier(participants);

                // Perform some more work
                Console.WriteLine("[{0}] Leaving for Boston", name);
            }
            catch (Exception ex)
            {
               Console.WriteLine("[{0}] Caravan was cancelled! Going home!", name);
               Console.WriteLine(ex);
            }
        }
    }
}
