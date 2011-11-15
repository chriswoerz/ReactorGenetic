using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ReactorGeneric
{
    public class Population
    {
        public IList<Reactor> ItsReactors { get; set; }
        private static int ItsTicksPerGeneration { get; set; }

        public Population(int populationSize, int chambersPer, int ticksPerGeneration, ManualResetEvent[] finishedTrials)
        {
            ItsTicksPerGeneration = ticksPerGeneration;
            var contents = "blarg";

            ItsReactors = new List<Reactor>();
            for (int i = 0; i < populationSize; i++)
            {
                var reactor = new Reactor(this, chambersPer, contents, finishedTrials[i]);
                Populator.RandomReactor(reactor);
                ItsReactors.Add(reactor);
            }
        }

        public string GenerateFitnessReport(int ticks)
        {
            var reportSF = new StringBuilder();
            reportSF.AppendLine("Final Report");

            var reactors = ItsReactors.ToList();
            reactors.Sort();

            foreach (var reactor in reactors)
            {
                reportSF.AppendLine(reactor.GetReport(ticks));
            }

            return reportSF.ToString();
        }

        public static void RunReactor(object state)
        {
            var reactor = (Reactor) state;

            for (int t = 0; t < ItsTicksPerGeneration; t++)
            {
                reactor.OnTick( );
            }
            reactor.MarkFinal();
        }
    }
}