using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactorGeneric
{
    public class Population
    {
        public IList<Reactor> ItsReactors { get; set; }

        public event EventHandler Pulse;

        public void OnPulse(EventArgs e)
        {
            EventHandler handler = Pulse;
            if (handler != null) handler(this, e);
        }

        public Population(int populationSize, int chambersPer)
        {
            var contents = "blarg";

            ItsReactors = new List<Reactor>();
            for (int i = 0; i < populationSize; i++)
            {
                ItsReactors.Add(new Reactor(this, chambersPer, contents));
            }
        }

        public string GenerateReport()
        {
            var reportSF = new StringBuilder();
            reportSF.AppendLine("Final Report");

            foreach (var reactor in ItsReactors)
            {
                reportSF.AppendLine(reactor.GetReport());
            }

            return reportSF.ToString();
        }
    }
}