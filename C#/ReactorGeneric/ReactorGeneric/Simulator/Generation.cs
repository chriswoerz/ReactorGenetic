using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactorGeneric.Component;

namespace ReactorGeneric.Simulator
{
    public class Generation
    {
        public IList<Reactor> ItsReactors { get; set; }
        private static int ItsTicksPerGeneration { get; set; }

        public Generation(int id, int ticks, int targetPopulation)
        {
            ItsMeltedReactors = new List<Reactor>();
            ItsSuccessfulReactors = new List<Reactor>();
            ItsUnpoweredReactors = new List<Reactor>();
            ItsId = id;
            Initialize(ticks, targetPopulation);
        }

        public int ItsId { get; set; }

        public Generation(int id, int populationSize, int chambers, int ticks, int externalCoolingPerTick):this(id, ticks, populationSize)
        {
            Initialize(ticks, populationSize);
            for (int i = 0; i < populationSize; i++)
            {
                ItsReactors.Add(new Reactor(this, chambers, externalCoolingPerTick));
            }
        }

        private void Initialize(int ticks, int targetPopulation)
        {
            ItsTicksPerGeneration = ticks;
            ItsReactors = new List<Reactor>(targetPopulation);

        }

        public Generation(int id, IEnumerable<ReactorResult> startingPopulation, int targetPopulation, int chambers, int ticks, int externalCoolingPerTick ):this(id, ticks, targetPopulation)
        {
            if (startingPopulation.Count() == 0) throw new ArgumentOutOfRangeException("startingPopulation", "cannot be 0");

            ItsReactors = Populator.Repopulate(this, startingPopulation.ToList(), targetPopulation, chambers, externalCoolingPerTick);
        }

        public string GenerateFitnessReport(int ticks)
        {
            var report = new StringBuilder();
            report.AppendLine(string.Format("{0}====Generation Report===={0}", Environment.NewLine));

            foreach (Reactor successfulReactor in ItsSuccessfulReactors)
            {
               report.AppendLine(successfulReactor.GetReport(ticks));
            }

            return report.ToString();
        }

        protected IList<Reactor> ItsMeltedReactors { get; set; }
        protected IList<Reactor> ItsUnpoweredReactors { get; set; }
        protected IList<Reactor> ItsSuccessfulReactors { get; set; }

        public static void RunReactor(Reactor reactor)
        {
            for (int t = 0; t < ItsTicksPerGeneration; t++)
            {
                reactor.OnTick();
            }
        }

        public IEnumerable<ReactorResult> GetSuccesses()
        {
            return ItsSuccessfulReactors.Select(reactor => reactor.GetResult());
        }

        public void Finialize()
        {
            var reactors = ItsReactors.ToList();
            reactors.Sort();
            reactors.Reverse();

            foreach (var reactor in reactors)
            {
                if (reactor.ItsMeltedFlag)
                {
                    ItsMeltedReactors.Add(reactor);
                }
                if (reactor.ItsPowerOutput == 0)
                {
                    ItsUnpoweredReactors.Add(reactor);
                }
                else
                {
                    ItsSuccessfulReactors.Add(reactor);
                }
            }
        }
    }
}