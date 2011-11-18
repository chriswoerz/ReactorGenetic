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

        public Generation(int id)
        {
            ItsMeltedReactors = new List<Reactor>();
            ItsSuccessfulReactors = new List<Reactor>();
            ItsId = id;
        }

        public int ItsId { get; set; }

        public Generation(int id, int populationSize, int chambers, int ticks):this(id)
        {
            Initialize(ticks, populationSize);
            for (int i = 0; i < populationSize; i++)
            {
                ItsReactors.Add(new Reactor(this, chambers));
            }
        }

        private void Initialize(int ticks, int targetPopulation)
        {
            ItsTicksPerGeneration = ticks;
            ItsReactors = new List<Reactor>(targetPopulation);
        }

        public Generation(int id, IEnumerable<ReactorResult> startingPopulation, int targetPopulation, int chambers, int ticks ):this(id)
        {
            if (startingPopulation.Count() == 0) throw new ArgumentOutOfRangeException("startingPopulation", "cannot be 0");

            Initialize(ticks, targetPopulation);
            ItsReactors = Populator.Repopulate(this, startingPopulation.ToList(), targetPopulation, chambers);
        }

        public string GenerateFitnessReport(int ticks)
        {
            var report = new StringBuilder();
            report.AppendLine("\n====Generation Report====\n");

            foreach (Reactor successfulReactor in ItsSuccessfulReactors)
            {
               report.AppendLine(successfulReactor.GetReport(ticks));
            }

            return report.ToString();
        }

        protected IList<Reactor> ItsMeltedReactors { get; set; }
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
                else
                {
                    ItsSuccessfulReactors.Add(reactor);
                }
            }
        }
    }
}