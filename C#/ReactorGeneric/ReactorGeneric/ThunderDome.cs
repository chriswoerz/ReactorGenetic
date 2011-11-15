using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace ReactorGeneric
{
    public static class ThunderDome
    {
        public static Population ItsPopulation { get; set; }

        public static event ReportEventHandler Report;

        public static void OnReport(ReportEventArgs e)
        {
            ReportEventHandler handler = Report;
            if (handler != null) handler(null, e);
        }


        public static void Start(int population, int chambers, int generations, int ticksPerGeneration)
        {
            var doneEvents = new ManualResetEvent[population];

            for (int index = 0; index < doneEvents.Length; index++)
            {
                doneEvents[index] = new ManualResetEvent(false);
            }
            ItsPopulation = new Population(population, chambers, ticksPerGeneration, doneEvents);

            foreach (Reactor reactor in ItsPopulation.ItsReactors)
            {
                ThreadPool.QueueUserWorkItem(Population.RunReactor, reactor);
            }

            WaitHandle.WaitAll(doneEvents);
            var reporttxt = ItsPopulation.GenerateFitnessReport(ticksPerGeneration);

            OnReport(new ReportEventArgs { ReportText = reporttxt });
        }

        private static bool ItsStopFlag { get; set; }

        public static void Pause()
        {
            throw new NotImplementedException();
        }

        public static void Stop()
        {
            ItsStopFlag = true;
        }
    }

    public static class Fitness
    {
        public static void Determine(ref IList<Reactor> reactors )
        {
            reactors.ToList().Sort();
        }
    }
}
