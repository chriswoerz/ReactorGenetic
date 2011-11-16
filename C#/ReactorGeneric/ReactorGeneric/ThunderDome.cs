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
            ItsPopulationCount = population;
            ItsChambers = chambers;
            ItsTicksPerGeneration = ticksPerGeneration;
            ItsCountdown = new CountdownEvent(1);

            var worker = new BackgroundWorker();
            worker.DoWork += StartWorker_DoWork;
            worker.RunWorkerCompleted += StartWorker_Complete;
            worker.RunWorkerAsync();
           
        }

        private static void StartWorker_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            OnReport(new ReportEventArgs("", ReportType.ReplaceLast));
            var reporttxt = ItsPopulation.GenerateFitnessReport(ItsTicksPerGeneration);

            OnReport(new ReportEventArgs(reporttxt, ReportType.Append) );
        }

        private static int ItsTicksPerGeneration { get; set; }

        private static int ItsChambers { get; set; }

        private static int ItsPopulationCount { get; set; }

        private static CountdownEvent ItsCountdown { get; set; }

        private static void StartWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ItsPopulation = new Population(ItsPopulationCount, ItsChambers, ItsTicksPerGeneration);

            OnReport(new ReportEventArgs(string.Format("Starting Population: {0}\n", ItsPopulationCount), ReportType.Append));

            foreach (Reactor reactor in ItsPopulation.ItsReactors)
            {
                ItsCountdown.AddCount();

                Reactor reactorState = reactor;
                ThreadPool.QueueUserWorkItem(
                  (state) =>
                  {
                      try
                      {
                          Population.RunReactor(reactorState);
                      }
                      finally
                      {
                          SignalOne();
                      }
                  });
            }
            SignalOne();
            ItsCountdown.Wait();
        }

        private static void SignalOne()
        {
            OnReport(new ReportEventArgs(string.Format("\nTodo: {0}", ItsCountdown.CurrentCount), ReportType.ReplaceLast));
            ItsCountdown.Signal();
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
