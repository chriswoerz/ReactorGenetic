using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using ReactorGeneric.Component;

namespace ReactorGeneric.Simulator
{
    public static class ThunderDome
    {
        private const bool RunThreaded = false;

        //public static Generation ItsGeneration { get; set; }

        public static List<ReactorResult> ItsRunResults { get; set; }

        public static event ReportEventHandler Report;

        public static void OnReport(ReportEventArgs e)
        {
            ReportEventHandler handler = Report;
            if (handler != null) handler(null, e);
        }


        public static void Start(int population, int chambers, int generations, int ticksPerGeneration, int externalCoolingPerTick)
        {
            ItsRunResults = new List<ReactorResult>();
            ItsReactorCount = population;
            ItsGenerationCount = generations;
            ItsChambers = chambers;
            ItsExternalCoolingPerTick = externalCoolingPerTick;
            ItsTicksPerGeneration = ticksPerGeneration;

            var worker = new BackgroundWorker();
            worker.DoWork += StartWorker_DoWork;
            worker.RunWorkerCompleted += StartWorker_Complete;
            worker.RunWorkerAsync();
           
        }

        private static int ItsExternalCoolingPerTick { get; set; }

        private static int ItsGenerationCount { get; set; }


        private static void StartWorker_DoWork(object sender, DoWorkEventArgs e)
        {
                OnReport(new ReportEventArgs(string.Format("Starting Population: {0}{1}", ItsReactorCount, Environment.NewLine), ReportType.Append));
            for (int i = 0; i < ItsGenerationCount; i++)
            {
                    RunGeneration();
            }
                OnReport(new ReportEventArgs(string.Format("{1}====Finished:===={1}{0}", PrettyPrintFinal(), Environment.NewLine), ReportType.Append));
        }

        private static string PrettyPrintFinal()
        {
            ItsRunResults.Sort();

            string toReturn = string.Empty;

            foreach (ReactorResult reactorResult in ItsRunResults.Take(ItsReactorCount/2))
            {
                toReturn += string.Format("EU: {0} EF: {1} M: {2}{3}", reactorResult.ItsEUOutput,
                                          reactorResult.ItsEfficency, reactorResult.ItsManifest, Environment.NewLine);
            }
            return toReturn;
        }

        private static void StartWorker_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
          //FINAL REPORT
        }

        private static int ItsTicksPerGeneration { get; set; }

        private static int ItsChambers { get; set; }

        private static int ItsReactorCount { get; set; }

        //private static CountdownEvent ItsCountdown { get; set; }

        private static void RunGeneration()
        {
            var countdown = new CountdownEvent(1);

            ItsCurrentGenerationId++;

            var initialPopulation = GetInitialPopulation();

            Generation generation;

            if (initialPopulation.Count() > 0)
                generation = new Generation(ItsCurrentGenerationId, initialPopulation, ItsReactorCount, ItsChambers, ItsTicksPerGeneration, ItsExternalCoolingPerTick);
            else
                generation = new Generation(ItsCurrentGenerationId, ItsReactorCount, ItsChambers, ItsTicksPerGeneration, ItsExternalCoolingPerTick);
            

            foreach (Reactor reactor in generation.ItsReactors)
            {
                if (RunThreaded)
                {
                    countdown.AddCount();

                    Reactor reactorState = reactor;
                    ThreadPool.QueueUserWorkItem(
                      (state) =>
                      {
                          try
                          {
                              Generation.RunReactor(reactorState);
                          }
                          finally
                          {
                              SignalOne(countdown, "Reactors");
                          }
                      });
                }
                else {
                    Generation.RunReactor(reactor);
                }
            }
            SignalOne(countdown, "Reactors");
            countdown.Wait();
            generation.Finialize();

            ItsRunResults.AddRange(generation.GetSuccesses());

            var reporttxt = generation.GenerateFitnessReport(ItsTicksPerGeneration);
            OnReport(new ReportEventArgs(reporttxt, ReportType.Append));
        }

        private static IEnumerable<ReactorResult> GetInitialPopulation()
        {
            var canidatePopulation = GetLastRunsSuccesses(ItsCurrentGenerationId);
            var bestSuccesses = GetBestSuccesses().ToList();

            if (canidatePopulation.Count() > ItsReactorCount / 10)
            {
                return canidatePopulation.Take(ItsReactorCount/3);
            }
            if (bestSuccesses.Count() > ItsReactorCount / 10)
            {
                return bestSuccesses.Take(ItsReactorCount/3);
            }

            return new List<ReactorResult>();
        }

        private static IEnumerable<ReactorResult> GetBestSuccesses()
        {
            ItsRunResults.Sort();
            return ItsRunResults.Take(100);
        }

        private static IEnumerable<ReactorResult> GetLastRunsSuccesses(int currentGenerationId)
        {
            if (ItsRunResults != null)
                return ItsRunResults.ToList().FindAll(r => r.ItsGenerationId == currentGenerationId - 1);

            return new List<ReactorResult>();
        }

        private static int ItsCurrentGenerationId { get; set; }

        private static void SignalOne(CountdownEvent countdown, string type)
        {
            OnReport(new ReportEventArgs(string.Format("{1}"+ type +" Left: {0}", countdown.CurrentCount, Environment.NewLine), ReportType.ReplaceLast));
            countdown.Signal();
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

    public class ReactorResult :IComparable<ReactorResult>
    {
        public string ItsManifest { get; set; }

        public int ItsEUOutput { get; set; }

        public int ItsHeat { get; set; }

        public int ItsEfficency { get; set; }

        public int ItsGenerationId { get; set; }

        public int CompareTo(ReactorResult other)
        {
            return this.ItsEfficency - other.ItsEfficency;
        }
    }

 
}
